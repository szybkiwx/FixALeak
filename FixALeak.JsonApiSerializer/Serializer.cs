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
        private IPropertySerializationContext _propertySerializationContext;

        public Serializer(IPropertySerializationContext propertySerializationContext)
        {
            _propertySerializationContext = propertySerializationContext;
        }

        public JObject Serialize(object obj, string include)
        {
            var resourceIdObject = new JsonResourceSerializeObject(obj);
            var serializedObject = SerializeObject(obj);
            JObject relationships = SerializeRelationships(obj);
            serializedObject.Add(new JProperty("relationships", relationships));
            return JObject.FromObject(new {
                links = new
                {
                    self = resourceIdObject.GetSelfLink().ToString()
                },
                data = serializedObject
            });
        }

        public JObject Serialize(IEnumerable collection, string include)
        {
            return Serialize(collection.Cast<object>().ToList(), include);
        }

        public JObject Serialize(ICollection collection, string include)
        {
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            var resourceIdObject = new JsonResourceSerializeObject(enumerator.Current);

            return JObject.FromObject(new
            {
                links = new
                {
                    self = resourceIdObject.GetSelfCollectionLink().ToString()
                },
                data = collection.Cast<object>().Select(x =>
                    {
                        var serializedObject = SerializeObject(x);
                        JObject relationships = SerializeRelationships(x);
                        serializedObject.Add(new JProperty("relationships", relationships));
                        return serializedObject;
                    })
            });
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

            var attributes = obj.GetType().GetProperties()
                .Where(prop => !prop.Name.EndsWith("ID") && (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)))
                .Select(prop => new JProperty(prop.Name.ToLower(), prop.GetValue(obj)));
                
            serializedObject.Add(new JProperty("attributes", new JObject(attributes)));
            return serializedObject;
        }
    }
}
