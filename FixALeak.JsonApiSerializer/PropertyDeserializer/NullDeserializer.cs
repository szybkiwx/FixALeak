using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public class RefPropertyDeserializer : IPropertyDeserializer
    {
        public object Deserialize(PropertyInfo property, JProperty rel, JToken data)
        {
            var relResourceObject = new OutResourceObject(rel.Value["data"], property.PropertyType);
            return relResourceObject.Instance;
        }
    }
}