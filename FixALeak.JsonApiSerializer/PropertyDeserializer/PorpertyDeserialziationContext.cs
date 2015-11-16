using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public class PorpertyDeserialziationContext : IPorpertyDeserialziationContext
    {
        private Dictionary<Func<PropertyInfo, bool>, IPropertyDeserializer> _strategies;

        public PorpertyDeserialziationContext(PropertyDeserilaizerAggregate aggregate)
        {
            _strategies = new Dictionary<Func<PropertyInfo, bool>, IPropertyDeserializer>();
            _strategies.Add(prop => prop.PropertyType.GetInterface("IEnumerable") !=null, aggregate.CollectionPropertySerializer);
            _strategies.Add(prop => prop.PropertyType.IsValueType, aggregate.ValuePropertySerializer);
            _strategies.Add(prop => true, aggregate.RefPropertyDeserializer);
        }

        public IPropertyDeserializer GetDeserializer(PropertyInfo property, JProperty data)
        {
            return _strategies.FirstOrDefault(x => x.Key.Invoke(property)).Value;
        }
    }
}
