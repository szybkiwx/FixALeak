using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class NullSerializer : IPropertySerializer
    {
        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            return null;
        }

        public IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop)
        {
            return null;
        }
    }
}
