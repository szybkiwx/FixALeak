using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class CollectionPropertySerializer : IPropertySerializer
    {
        private ISingleObjectSerializer _singleObjectSerializer;

        public CollectionPropertySerializer(ISingleObjectSerializer singleObjectSerializer)
        {
            _singleObjectSerializer = singleObjectSerializer;
        }


        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            JArray array = new JArray(); 

            if (prop.GetValue(obj) != null)
            {
                if (prop.PropertyType.GetInterface("ICollection") != null)
                {
                    var collection = prop.GetValue(obj) as ICollection;
                    array = new JArray(collection.Cast<object>().Select(x => new InResourceObject(x).GetJObject()));
                }
                else if (prop.PropertyType.GetInterface("IEnumerable") != null)
                {
                    var collection = prop.GetValue(obj) as IEnumerable;
                    array = new JArray(collection.Cast<object>().Select(x => new InResourceObject(x).GetJObject()));
                }
            }
            
            var resourceIdObject = new InResourceObject(obj);
            Type genericType = prop.PropertyType.GetGenericArguments().First();
            string relationshipName = genericType.Name.ToLower();

            return new JProperty(prop.Name.ToLower(), JObject.FromObject(new
            {
                data = array,
                links = new
                {
                    self = resourceIdObject.GetRelatedSelfLink(relationshipName).ToString(),
                    related = resourceIdObject.GetRelatedLink(relationshipName).ToString()
                }
            }));
        }


        public IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop)
        {
            IEnumerable<JObject> result;
            if (prop.PropertyType.GetInterface("ICollection") != null)
            {
                var collection = prop.GetValue(obj) as ICollection;
                result = collection.Cast<object>().Select(x => _singleObjectSerializer.Serialize(x));
                //result = prop.GetValue(obj).Cast<object>().Select(x => _singleObjectSerializer.Serialize(x));
            }
            else if (prop.PropertyType.GetInterface("IEnumerable") != null)
            {
                var collection = prop.GetValue(obj) as IEnumerable;
                result = collection.Cast<object>().Select(x => _singleObjectSerializer.Serialize(x));
            }
            else
            {
                result = new List<JObject>();
            }

            return result;
        }
    }
}
