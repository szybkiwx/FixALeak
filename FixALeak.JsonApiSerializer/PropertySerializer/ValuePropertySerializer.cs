﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class ValuePropertySerializer : IPropertySerializer
    {
        private ISingleObjectSerializer _singleObjectSerializer;

        public ValuePropertySerializer(ISingleObjectSerializer singleObjectSerializer)
        {
            _singleObjectSerializer = singleObjectSerializer;
        }
        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            var resourceIdObject = new InResourceObject(obj);
            var relatedIdObject = new InResourceObject(prop.GetValue(obj));
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

        public IEnumerable<JObject> SerializeFull(object obj, PropertyInfo prop)
        {
            return new List<JObject>() { _singleObjectSerializer.Serialize(prop.GetValue(obj)) };
        }

    }
}
