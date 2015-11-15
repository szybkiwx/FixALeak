using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer
{

    public class JsonApiPatch
    {
        private Dictionary<PropertyInfo, object> propertyMap;

        public JsonApiPatch()
        {
            propertyMap = new Dictionary<PropertyInfo, object>();
        }

        public void SetValue(PropertyInfo property, object value)
        {
            propertyMap.Add(property, value);
        }

        public void Patch(object obj)
        {
            foreach (var kv in propertyMap)
            {
                var property = obj.GetType().GetProperties().FirstOrDefault(prop => prop.Name == kv.Key.Name);
                if (property != null)
                {
                    property.SetValue(obj, kv.Value, null);
                }

            }
        }
    }

    public class JsonApiPatch<T> : JsonApiPatch where T : new()
    {
        public void SetValue(Expression<Func<T, object>> setter, object value)
        {
            Expression exp = setter.Body;
            if (setter.Body.NodeType == ExpressionType.Convert || setter.Body.NodeType == ExpressionType.ConvertChecked)
            {
                var ue = setter.Body as UnaryExpression;
                exp = ue.Operand;

            }
            var me = exp as MemberExpression;
            var property = me.Member as PropertyInfo;
            SetValue(property, value);
        }

        public void Patch(T obj)
        {
            base.Patch(obj);
        }

       
    }
}
