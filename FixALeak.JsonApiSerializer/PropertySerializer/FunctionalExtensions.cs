using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public static class FunctionalExtensions
    {
        public static JProperty Serialize<T>(this IPropertySerializer serializer, object obj, Expression<Func<T, object>> prop)
        {
            return serializer.Serialize(obj, prop);
        }

        public static IEnumerable<JObject> SerializeFull<T>(this IPropertySerializer serializer, object obj, Expression<Func<T, object>> prop)
        {
            return serializer.SerializeFull(obj, prop);
        }
    }
}
