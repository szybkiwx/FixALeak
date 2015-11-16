using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public class KeyPropertyDeserializer : IPropertyDeserializer
    {
        public object Deserialize(PropertyInfo property, JProperty rel, JToken data)
        {
            throw new NotImplementedException();
        }
    }
}