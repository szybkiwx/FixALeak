using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer
{
    public static class ExpressionToPropertyInfoConverter
    {
        public static PropertyInfo Convert<T>(Expression<Func<T, object>> exppression)
        {
            Expression body = exppression.Body;
            if (exppression.Body.NodeType == ExpressionType.Convert || exppression.Body.NodeType == ExpressionType.ConvertChecked)
            {
                var ue = exppression.Body as UnaryExpression;
                body = ue.Operand;

            }
            var me = body as MemberExpression;
            return me.Member as PropertyInfo;
        }
    }
}
