using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
namespace FixALeak.JsonApiSerializer
{
    public class Serializer
    {
        private static PluralizationService PluralizationService
        {
            get 
            {
                return PluralizationService.CreateService(new CultureInfo("en-US"));
            }
        }

        public JObject Serialize(object obj)
        {
            JObject root = new JObject();
            root.Add("links", JObject.FromObject( new {
                self = PluralizationService.Pluralize(obj.GetType().Name.ToLower()) + "/" + ExtractID(obj)
            }));
            
            var serializedObject = SerializeObject(obj);
            JObject relationships = SerializeRelationships(obj);
            serializedObject.Add(new JProperty("relationships", relationships));
            root.Add("data", serializedObject);

            return root;
        }

        public JObject SerializeBasicData(object obj)
        {
            JObject serializedObject = new JObject();
            Type type = obj.GetType();
            serializedObject.Add("type", type.Name.ToLower());
            serializedObject.Add(new JProperty("id", ExtractID(obj) /*type.GetProperty("ID").GetValue(obj)*/));
            return serializedObject;
        }

        private JObject SerializeSingleProperty(object obj, PropertyInfo prop)
        {
            var relatedProp = obj.GetType().GetProperties()
                       .SingleOrDefault(p => p.GetCustomAttributes(true)
                           .ToList().Exists(a => a.GetType()
                               .IsAssignableFrom(typeof(ForeignKeyAttribute))));

            string objectTypeName = obj.GetType().Name.ToLower();
            string properyTypeName = prop.Name.Substring(0, prop.Name.Length - 2);
            string relationshipName = (relatedProp != null ? relatedProp.Name : properyTypeName).ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append( PluralizationService.Pluralize(objectTypeName));
            sb.Append("/");
            sb.Append(ExtractID(obj));
            sb.Append("/");
            sb.Append(PluralizationService.Pluralize(relationshipName));

            return JObject.FromObject(new
                {
                    data = new {
                        id = prop.GetValue(obj),
                        type = relationshipName
                    },
                    links = new  {
                        //self = "",
                        related = sb.ToString()
                    }
                });
        }

        private JObject SerializeCollectionProperty(object obj, PropertyInfo prop)
        {
            var collection = prop.GetValue(obj) as ICollection;
            JArray array = new JArray();
            if (collection != null)
            {
                foreach(var r in collection) {
                    array.Add(JObject.FromObject(new {
                        id = ExtractID(r) /* r.GetType().GetProperty("ID").GetValue(r)*/,
                        type = r.GetType().Name.ToLower()
                    }));
                }
            }
            
            return JObject.FromObject(new
            {
                data = array
            });
        }

        public JObject SerializeRelationships(object obj)
        {
            JObject relationships = new JObject();
            Type type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                string typeName = prop.Name.Substring(0, prop.Name.Length - 2);
                if (prop.Name.EndsWith("ID") && prop.Name.Length > 2)
                {
                    relationships.Add(new JProperty(typeName.ToLower(), SerializeSingleProperty(obj, prop)));
                }
                else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    relationships.Add(new JProperty(prop.Name.ToLower(), SerializeCollectionProperty(obj, prop)));
                }
            }

            return relationships;
        }

        public JObject SerializeObject(object obj)
        {
            JObject serializedObject = SerializeBasicData(obj);
            Type type = obj.GetType();
            JObject attributes = new JObject();
            foreach (var prop in type.GetProperties())
            {
                if (!prop.Name.EndsWith("ID") && prop.GetType().IsValueType)
                {
                    attributes.Add(new JProperty(prop.Name.ToLower(), prop.GetValue(obj)));
                }
            }

            serializedObject.Add(new JProperty("attributes", attributes));
            return serializedObject;
        }

        
        private int ExtractID(object obj)
        {
            return Int32.Parse(obj.GetType().GetProperty("ID").GetValue(obj).ToString());
        }
    }
}
