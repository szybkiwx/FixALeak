using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public interface IPorpertyDeserialziationContext
    {
        IPropertyDeserializer GetDeserializer(PropertyInfo property, JProperty data);
    }
}
