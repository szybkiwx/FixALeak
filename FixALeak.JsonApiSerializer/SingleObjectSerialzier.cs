using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer
{
    public class SingleObjectSerializer : ISingleObjectSerializer
    {
        public JObject Serialize(object obj)
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
