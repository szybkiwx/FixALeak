using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using FixALeak.JsonApiSerializer.PropertySerializer;
namespace FixALeak.JsonApiSerializer
{
    public class Serializer
    {
        private IPropertySerializationContext _propertySerializationContext;
        private ISingleObjectSerializer _singleObjectSerializer;

        public Serializer(IPropertySerializationContext propertySerializationContext, ISingleObjectSerializer singleObjectSerializer)
        {
            _propertySerializationContext = propertySerializationContext;
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
                .Select(prop => _propertySerializationContext.Serialize(obj, prop))
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
                                includes.AddRange(_propertySerializationContext.SerializeFull(obj, prop));

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
                               includes.AddRange(_propertySerializationContext.SerializeFull(obj, prop));

                           });
                }

                return includes;
            }
        }

        public object Deserialize(string json, Type returnType)
        {
            var rootNode = JObject.Parse(json);
            var dataNode = rootNode["data"];
            var resourceObject = _GetMainObject(dataNode, returnType);
            var relationships = dataNode["relationships"].AsEnumerable();
            if (relationships != null)
            {
                returnType.GetProperties().ToList().ForEach(prop =>
                {
                    string propName = prop.Name.ToLower();
                    foreach (var rel in relationships.Cast<JProperty>())
                    {
                        if (rel.Name == propName)
                        {
                            if (rel.Value["data"] is JArray)
                            {

                                if (prop.PropertyType.GetInterface("IEnumerable") != null)
                                {
                                    var instance = CollectionFactory.CreateCollectionInstance(prop.PropertyType);
                                    Type genericType = prop.PropertyType.GetGenericArguments()[0];
                                    ((JArray)rel.Value["data"]).ToList().ForEach(collectionItem =>
                                    {
                                        var item = new OutResourceObject(collectionItem, genericType);
                                        instance.Add(item.Instance);

                                    });

                                    prop.SetValue(resourceObject.Instance, instance, null);
                                }
                                else
                                {
                                    throw new CollectionTypeNotSupported();
                                }
                            }
                            else if (prop.PropertyType.IsValueType)
                            {
                                prop.SetValue(resourceObject.Instance, Convert.ChangeType(dataNode["attributes"][propName].ToString(), prop.PropertyType));
                            }
                            else
                            {
                                var relResourceObject = new OutResourceObject(rel.Value["data"], prop.PropertyType);
                                prop.SetValue(resourceObject.Instance, relResourceObject.Instance);
                            }
                        }
                        else if (rel.Name + "id" == propName && (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(Guid)))
                        {
                            prop.SetValue(resourceObject.Instance, Convert.ChangeType(rel.Value["data"]["id"].ToString(), prop.PropertyType));
                        }
                    };
                });
            }

            return Convert.ChangeType(resourceObject.Instance, returnType);
        } 

        public T Deserialize<T>(string json)
                where T : class, new()

        {
            return Deserialize(json, typeof(T)) as T;
        }
        

        private OutResourceObject _GetMainObject(JToken dataNode, Type returnType)
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
                        prop.SetValue(resourceObject.Instance, Convert.ChangeType(attributes[propName].ToString(), prop.PropertyType));
                    }
                });
            }

            return resourceObject;
        }


    }
}
