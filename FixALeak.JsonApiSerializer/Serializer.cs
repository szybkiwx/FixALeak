using FixALeak.JsonApiSerializer.PropertyDeserializer;
using FixALeak.JsonApiSerializer.PropertySerializer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer
{
    public class Serializer
    {
        private IPropertySerializationContext _propertySerializationContext;
        private IPorpertyDeserialziationContext _propertyDeserializationContext;
        private ISingleObjectSerializer _singleObjectSerializer;

        public Serializer(IPropertySerializationContext propertySerializationContext,
            IPorpertyDeserialziationContext propertyDeserializationContext,
            ISingleObjectSerializer singleObjectSerializer)
        {
            _propertySerializationContext = propertySerializationContext;
            _propertyDeserializationContext = propertyDeserializationContext;
            _singleObjectSerializer = singleObjectSerializer;
        }

        public JObject Serialize(object obj, string include = "")
        {
            var resourceIdObject = new InResourceObject(obj);
            var serializedObject = _singleObjectSerializer.Serialize(obj);
            JObject relationships = SerializeRelationships(obj);
            serializedObject.Add(new JProperty("relationships", relationships));
            return JObject.FromObject(new
            {
                links = new
                {
                    self = resourceIdObject.GetSelfLink().ToString()
                },
                data = serializedObject,
                included = GetIncludes(obj, include)
            });
        }

        public JObject Serialize(IEnumerable collection, string include = "")
        {
            return Serialize(collection.Cast<object>().ToList(), include);
        }

        public JObject Serialize(ICollection collection, string include = "")
        {
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            var resourceIdObject = new InResourceObject(enumerator.Current);

            return JObject.FromObject(new
            {
                links = new
                {
                    self = resourceIdObject.GetSelfCollectionLink().ToString()
                },
                data = collection.Cast<object>().Select(x =>
                    {
                        var serializedObject = _singleObjectSerializer.Serialize(x);
                        JObject relationships = SerializeRelationships(x);
                        serializedObject.Add(new JProperty("relationships", relationships));
                        return serializedObject;
                    }),
                included = collection.Cast<object>()
                                .Select(x => GetIncludes(x, include))
                                .Aggregate((aggregate, current) => aggregate.Union(current))
            });
        }
        
        private JObject SerializeRelationships(object obj)
        {
            Type type = obj.GetType();

            var serializedCollection = obj.GetType().GetProperties()
                .Select(prop => _propertySerializationContext.GetSerializer(obj, prop).Serialize(obj, prop))
                .Where(x => x != null)
                .GroupBy(x => x.Name)
                .Select(g => g.First());

            return new JObject(serializedCollection);
        }

        private IEnumerable<JObject> GetIncludes(object obj, string include)
        {
            {
                var includes = new List<JObject>();
                if (!string.IsNullOrWhiteSpace(include))
                {
                    include.Split(',').ToList().ForEach(toInclude =>
                    {
                        obj.GetType()
                            .GetProperties()
                            .Where(prop => prop.Name.ToLower() == toInclude)
                            .ToList()
                            .ForEach(prop =>
                            {
                                includes.AddRange(_propertySerializationContext.GetSerializer(obj, prop).SerializeFull(obj, prop));

                            });
                    });
                }
                else
                {
                    obj.GetType()
                           .GetProperties()
                           .Where(prop => prop.GetValue(obj) != null )
                           //.Where(prop => !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string))
                           .Where(prop => prop.PropertyType.IsClass || prop.PropertyType.GetInterface("IEnumerable") != null)
                           .Where(prop => prop.PropertyType != typeof(string))
                           .ToList()
                           .ForEach(prop =>
                           {
                               includes.AddRange(_propertySerializationContext.GetSerializer(obj, prop).SerializeFull(obj, prop));

                           });
                }

                return includes;
            }
        }

        public JsonApiPatch<T> DeserializePatch<T>(string json) where T : new()
        {
            var patch = new JsonApiPatch<T>();

            var rootNode = JObject.Parse(json);
            var dataNode = rootNode["data"];
            var resourceObject = _GetMainObject(dataNode, typeof(T), (prop, instance, value) =>
            {
                patch.SetValue(prop, value);
            });
            var relationships = dataNode["relationships"];
            if(relationships != null)
            {
                throw new RelationshipUpdateForbiddenException();
            }
            return patch;
        }

        public object DeserializePatch(string json, Type returnType)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(string json, Type returnType)
        {
            return Deserialize(json, returnType, (prop, instance, value) =>
            {
                prop.SetValue(instance, value, null);
            });

        }

        private object Deserialize(string json, Type returnType, Action<PropertyInfo, object, object> setter)
        { 
            var rootNode = JObject.Parse(json);
            var dataNode = rootNode["data"];
            var resourceObject = _GetMainObject(dataNode, returnType, setter);
            var relationships = dataNode["relationships"].AsEnumerable();
            if (relationships != null)
            {
                _DeserializeRelationships(returnType, setter, dataNode, resourceObject, relationships);
            }

            return Convert.ChangeType(resourceObject.Instance, returnType);
        }

        private void _DeserializeRelationships(Type returnType, Action<PropertyInfo, object, object> setter, JToken dataNode, OutResourceObject resourceObject, IEnumerable<JToken> relationships)
        {
            var relationshipsByKey = relationships.Cast<JProperty>()
                                .ToDictionary(x => x.Name, x => x);

            var properties = returnType.GetProperties()
                .Where(x => relationshipsByKey.Keys.Contains(x.Name.ToLower())).ToList();

            var idProperties = returnType.GetProperties()
                .Where(x => x.Name.ToLower().EndsWith("id"))
                .Where(x => relationshipsByKey.Keys.Contains(x.Name.ToLower().Replace("id", "")))
                .Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(Guid));


            idProperties.ToList().ForEach(prop =>
            {
                var rel = relationshipsByKey[prop.Name.ToLower().Replace("id", "")];
                setter.Invoke(prop, resourceObject.Instance, Convert.ChangeType(rel.Value["data"]["id"].ToString(), prop.PropertyType));
            });

            properties.ToList().ForEach(prop =>
            {
                var rel = relationshipsByKey[prop.Name.ToLower()];
                var deserializedObject = _propertyDeserializationContext.GetDeserializer(prop, rel).Deserialize(prop, rel, dataNode);

                setter.Invoke(prop, resourceObject.Instance, deserializedObject);
            });
        }

        public T Deserialize<T>(string json)
                where T : class, new()

        {
            return Deserialize(json, typeof(T)) as T;
        }
        

        private OutResourceObject _GetMainObject(JToken dataNode, Type returnType, Action<PropertyInfo, object, object> setter)
        {
            var resourceObject = new OutResourceObject(dataNode, returnType);

            var attributes = dataNode["attributes"];
            if (attributes != null)
            {
                returnType.GetProperties().ToList().ForEach(prop =>
                {
                    string propName = prop.Name.ToLower();
                    if (attributes[propName] != null)
                    {

                        setter.Invoke(prop, resourceObject.Instance, Convert.ChangeType(attributes[propName].ToString(), prop.PropertyType));
                        /*prop.SetValue(resourceObject.Instance, 
                            Convert.ChangeType(attributes[propName].ToString(), prop.PropertyType), 
                            null);*/
                    }
                });
            }

            return resourceObject;
        }


    }
}
