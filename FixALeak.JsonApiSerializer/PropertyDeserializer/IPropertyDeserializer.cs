using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public interface IPropertyDeserializer
    {
        object Deserialize(PropertyInfo property, JProperty rel, JToken data);
    }
}
