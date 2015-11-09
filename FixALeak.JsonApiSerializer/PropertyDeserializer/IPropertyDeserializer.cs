using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public interface IPropertyDeserializer
    {
        object Deserialize(PropertyInfo property, JProperty rel, JToken data);
    }
}
