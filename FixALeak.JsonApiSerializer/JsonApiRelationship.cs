using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FixALeak.JsonApiSerializer
{
    public class JsonApiRelationship
    {
        public object Value { get; protected set; }
        public PropertyInfo Property { get; protected set; }
    }

    public class JsonApiRelationship<T> : JsonApiRelationship
    {
        private JsonApiRelationship()
        {
        }

        public static JsonApiRelationship<T> Create(T value, Expression<Func<T, object>> property)
        {
            var rel = new JsonApiRelationship<T>();
            rel.Value = value;
            rel.Property = ExpressionToPropertyInfoConverter.Convert(property);
            return rel;
        }
    }
}
