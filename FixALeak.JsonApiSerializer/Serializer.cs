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
using FixALeak.JsonApiSerializer.PropertySerializer;
namespace FixALeak.JsonApiSerializer
{
    
    public class Serializer
    {
        private PropertySerializationContext _propertySerializationContext;

        public Serializer()
        {
            _propertySerializationContext = new PropertySerializationContext();
        }

        public JObject Serialize(object obj)
        {
            JObject root = new JObject();
            var resourceIdObject = new JsonResourceSerializeObject(obj);
            root.Add("links", JObject.FromObject(new
            {
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

        private JObject SerializeRelationships(object obj)
        {
            Type type = obj.GetType();

            var serializedCollection = obj.GetType().GetProperties()
                .Select(prop => _propertySerializationContext.Serialize(obj, prop))
                .Where(x => x != null)
                .GroupBy(x => x.Name)
                .Select(g => g.First());
                
            return new JObject(serializedCollection);
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
