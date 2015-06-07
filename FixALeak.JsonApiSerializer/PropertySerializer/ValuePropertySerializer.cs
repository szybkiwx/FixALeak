using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class ValuePropertySerializer : IPropertySerializer
    {
        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            var resourceIdObject = new JsonResourceSerializeObject(obj);
            var relatedIdObject = new JsonResourceSerializeObject(prop.GetValue(obj));
            return new JProperty(prop.Name.ToLower(), JObject.FromObject(new
            {
                data = new
                {
                    id = relatedIdObject.ID,
                    type = relatedIdObject.TypeName
                },
                links = new
                {
                    self = resourceIdObject.GetRelatedSelfLink(relatedIdObject.TypeName, relatedIdObject.ID).ToString(),
                    related = resourceIdObject.GetRelatedLink(relatedIdObject.TypeName, relatedIdObject.ID).ToString()
                }
            }));
        }
    }
}
