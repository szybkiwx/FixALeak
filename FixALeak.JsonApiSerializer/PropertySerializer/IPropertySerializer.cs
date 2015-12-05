using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public interface IPropertySerializer
    {
        JProperty Serialize(object obj, PropertyInfo prop);

        IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop);
    }
}
