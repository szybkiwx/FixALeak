using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class NullSerializer : IPropertySerializer
    {
        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            return null;
        }
    }
}
