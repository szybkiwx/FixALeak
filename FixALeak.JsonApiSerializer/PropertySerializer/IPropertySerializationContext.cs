using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public interface IPropertySerializationContext
    {

        //JProperty Serialize(object obj, PropertyInfo prop);

        //IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop);

        IPropertySerializer GetSerializer(object obj, PropertyInfo prop);
    }
}
