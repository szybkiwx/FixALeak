using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public class ValuePropertyDeserializer : IPropertyDeserializer
    {
        public object Deserialize(PropertyInfo property, JProperty rel, JToken data)
        {
            return Convert.ChangeType(data["attributes"][property.Name.ToLower()].ToString(), property.PropertyType);
        }
    }
}