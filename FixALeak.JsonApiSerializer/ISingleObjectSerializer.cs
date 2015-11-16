using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer
{
    public interface ISingleObjectSerializer
    {
        JObject Serialize(object obj);

    }
}
