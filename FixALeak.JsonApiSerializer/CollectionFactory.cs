using System;
using System.Collections;
using System.Collections.Generic;

namespace FixALeak.JsonApiSerializer
{
    public class CollectionFactory
    {
        public static IList CreateCollectionInstance(Type type)
        {
            if(type.GetInterface("IEnumerable") == null)
            {
                throw new ArgumentException("Not an enumerable type: " + typeof(Type));
            }

            Type genericType = type.GetGenericArguments()[0];

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(genericType);

            return (IList)Activator.CreateInstance(constructedListType);
            
        }
    }
}
