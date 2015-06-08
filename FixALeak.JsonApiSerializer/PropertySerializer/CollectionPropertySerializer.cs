using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class CollectionPropertySerializer : IPropertySerializer
    {
        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            var collection = prop.GetValue(obj) as ICollection;
            JArray array = new JArray();

            if (collection != null)
            {
                array = new JArray(collection.Cast<object>().Select(x => new JsonResourceSerializeObject(x).GetJObject()));
            }

            var resourceIdObject = new JsonResourceSerializeObject(obj);
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
    }
}
