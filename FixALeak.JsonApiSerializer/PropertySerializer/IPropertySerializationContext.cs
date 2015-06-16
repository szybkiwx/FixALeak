using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public interface IPropertySerializationContext
    {
        JProperty Serialize(object obj, PropertyInfo prop);

        IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop);
    }
}
