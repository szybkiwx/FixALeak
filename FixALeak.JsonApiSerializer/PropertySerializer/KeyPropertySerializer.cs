using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;


namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class KeyPropertySerializer : IPropertySerializer
    {
        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            var relatedProp = GetRelated(obj);
            if (relatedProp == null)
            {
                return null;
            }

            var resourceIdObject = new ResourceObject(obj);
            string properyTypeName = prop.Name.Substring(0, prop.Name.Length - 2);
            string relationshipName = (relatedProp != null ? relatedProp.Name : properyTypeName).ToLower();

            int id = Int32.Parse(prop.GetValue(obj).ToString());

            return new JProperty(prop.Name.Substring(0, prop.Name.Length-2).ToLower(), JObject.FromObject(new
            {
                data = new
                {
                    id = id,
                    type = relationshipName
                },
                links = new
                {
                    self = resourceIdObject.GetRelatedSelfLink(relationshipName, id).ToString(),
                    related = resourceIdObject.GetRelatedLink(relationshipName, id).ToString()
                }
            }));
        }

        private PropertyInfo GetRelated(object obj)
        {
            return  obj.GetType().GetProperties()
                       .SingleOrDefault(p => p.GetCustomAttributes(true)
                           .ToList().Exists(a => a.GetType()
                               .IsAssignableFrom(typeof(ForeignKeyAttribute))));

        }


        public IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop)
        {
            throw new NotImplementedException();
        }
    }
}
