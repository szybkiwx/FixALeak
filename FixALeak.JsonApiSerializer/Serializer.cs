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
        
        public JObject Serialize(object obj)
        {
            JObject root = new JObject();
            var resourceIdObject = new JsonResourceSerializeObject(obj);
            root.Add("links", JObject.FromObject( new {
                self = resourceIdObject.GetSelfLink().ToString()
            }));
            
            var serializedObject = SerializeObject(obj);
            JObject relationships = SerializeRelationships(obj);
            serializedObject.Add(new JProperty("relationships", relationships));
            root.Add("data", serializedObject);

            return root;
        }

        public JObject Serialize(ICollection collection)
        {
            JObject root = new JObject();
            
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            var resourceIdObject = new JsonResourceSerializeObject(enumerator.Current);

            root.Add("links", JObject.FromObject(new
            {
                self = resourceIdObject.GetSelfCollectionLink().ToString()
            }));

            JArray list = new JArray();
            foreach (var obj in collection)
            {
                var serializedObject = SerializeObject(obj);
                JObject relationships = SerializeRelationships(obj);
                serializedObject.Add(new JProperty("relationships", relationships));
                list.Add(serializedObject);
            }
            root.Add("data", list);

            return root;
        }

        private JObject SerializeSingleProperty(object obj, PropertyInfo prop)
        {
            var relatedProp = obj.GetType().GetProperties()
                       .SingleOrDefault(p => p.GetCustomAttributes(true)
                           .ToList().Exists(a => a.GetType()
                               .IsAssignableFrom(typeof(ForeignKeyAttribute))));

            var resourceIdObject = new JsonResourceSerializeObject(obj);
            string properyTypeName = prop.Name.Substring(0, prop.Name.Length - 2);
            string relationshipName = (relatedProp != null ? relatedProp.Name : properyTypeName).ToLower();
            
            int id = Int32.Parse( prop.GetValue(obj).ToString());

            return JObject.FromObject(new
                {
                    data = new {
                        id =id,
                        type = relationshipName
                    },
                    links = new  {
                        self = resourceIdObject.GetRelatedSelfLink(relationshipName, id).ToString(),
                        related = resourceIdObject.GetRelatedLink(relationshipName, id).ToString()
                    }
                });
        }

        private JObject SerializeCollectionProperty(object obj, PropertyInfo prop)
        {
            var collection = prop.GetValue(obj) as ICollection;
            JArray array = new JArray();
            
            if (collection != null)
            {
                foreach (var elem in collection)
                {
                    array.Add(new JsonResourceSerializeObject(elem).GetJObject());
                } 
                //array = new JArray(collection.Select(x => new JsonResourceSerializeObject(x).GetJObject()));
            }
            var resourceIdObject = new JsonResourceSerializeObject(obj);
            Type genericType = prop.PropertyType.GetGenericArguments()[0];
            string relationshipName = genericType.Name.ToLower();

            return JObject.FromObject(new
            {
                data = array,
                links = new {
                    self = resourceIdObject.GetRelatedSelfLink(relationshipName).ToString(),
                    related = resourceIdObject.GetRelatedLink(relationshipName).ToString()
                }
            });
        }

        private JObject SerializeRelationships(object obj)
        {
            JObject relationships = new JObject();
            Type type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                string typeName = prop.Name.ToLower().Substring(0, prop.Name.Length - 2);
                if (prop.Name.EndsWith("ID") && prop.Name.Length > 2)
                {
                    relationships.Add(new JProperty(typeName.ToLower(), SerializeSingleProperty(obj, prop)));
                }
                else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                {
                    relationships.Add(new JProperty(prop.Name.ToLower(), SerializeCollectionProperty(obj, prop)));
                }
            }

            return relationships;
        }

        private JObject SerializeObject(object obj)
        {
            var resourceIdObject = new JsonResourceSerializeObject(obj);
            JObject serializedObject = resourceIdObject.GetJObject();
            Type type = obj.GetType();
            JObject attributes = new JObject();
            foreach (var prop in type.GetProperties())
            {
                if (!prop.Name.EndsWith("ID") && (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)))
                {
                    attributes.Add(new JProperty(prop.Name.ToLower(), prop.GetValue(obj)));
                }
            }

            serializedObject.Add(new JProperty("attributes", attributes));
            return serializedObject;
        }
    }
}
