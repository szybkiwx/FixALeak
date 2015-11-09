using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public interface IPorpertyDeserialziationContext
    {
        IPropertyDeserializer GetDeserializer(PropertyInfo property, JProperty data);
    }
}
