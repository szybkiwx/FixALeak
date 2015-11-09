using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public class CollectionPropertyDeserializer : IPropertyDeserializer
    {
        public object Deserialize(PropertyInfo property, JProperty rel, JToken data)
        {
            var instance = CollectionFactory.CreateCollectionInstance(property.PropertyType);
            Type genericType = property.PropertyType.GetGenericArguments()[0];


            ((JArray)rel.Value["data"]).ToList().ForEach(collectionItem =>
            {
                var item = new OutResourceObject(collectionItem, genericType);
                instance.Add(item.Instance);

            });

            return instance;
        }
    }
}
