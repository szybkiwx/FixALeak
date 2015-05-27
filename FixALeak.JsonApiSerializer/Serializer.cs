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
namespace FixALeak.JsonApiSerializer
{
    public class Serializer
    {
        public JObject Serialize(object obj)
        {
            JObject root = new JObject();
            JObject data = SerializeObject(obj);
            root.Add("data", data);
            return root;
        }

        /*public static JObject SerializeCollection(IEnumerable enumerable)
        {

        }*/


        public JObject SerializeIbjectWithRelationships(object obj)
        {
            var serializedObject = SerializeObject(obj);

            JObject relationships = SerializeRelationships(obj);

            serializedObject.Add(new JProperty("relationships", relationships));

            return serializedObject;
        }

        public JObject SerializeBasicData(object obj)
        {
            JObject serializedObject = new JObject();
            Type type = obj.GetType();
            serializedObject.Add("type", type.Name);
            serializedObject.Add(new JProperty("id", type.GetProperty("ID").GetValue(obj)));
            
            return serializedObject;
        }

        private JObject SerializeSinglePropert(object obj, PropertyInfo prop)
        {
            var relatedProp = obj.GetType().GetProperties()
                       .SingleOrDefault(p => p.GetCustomAttributes(true)
                           .ToList().Exists(a => a.GetType()
                               .IsAssignableFrom(typeof(ForeignKeyAttribute))));

            string typeName = prop.Name.Substring(0, prop.Name.Length - 2);
            string relationshipName = (relatedProp != null ? relatedProp.Name : typeName).ToLower();
            return JObject.FromObject(new
                    {
                        data = new {
                            id = prop.GetValue(obj),
                            type = relationshipName
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
                        id = r.GetType().GetProperty("ID").GetValue(r),
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
                    relationships.Add(new JProperty(typeName.ToLower(), SerializeSinglePropert(obj, prop)));
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
    }
}
