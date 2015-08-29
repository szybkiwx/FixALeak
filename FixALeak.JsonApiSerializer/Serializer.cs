using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
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

        public T Deserialize<T>(string json)
             where T : class, new() 

        {
            var rootNode = JObject.Parse(json);
            var dataNode = rootNode["data"];
            var typeNode = dataNode["type"];

            var resourceObject = new OutResourceObject(dataNode);
     
            var attributes = dataNode["attributes"];
            if (attributes != null)
            {
                resourceObject.ObjectType.GetProperties().ToList().ForEach(prop =>
                {
                    string propName = prop.Name.ToLower();
                    if (attributes[propName] != null)
                    {
                        prop.SetValue(resourceObject.Instance, Convert.ChangeType(attributes[propName].ToString(), prop.PropertyType));
                    }
                });
            }
           

            var relationships = dataNode["relationships"].AsEnumerable();

            if (relationships != null)
            {
                resourceObject.ObjectType.GetProperties().ToList().ForEach(prop =>
                {
                    string propName = prop.Name.ToLower();
                    foreach(var rel in relationships.Cast<JProperty>())
                    {
                        if (rel.Name == propName)
                        {
                            if (rel.Value["data"] is JArray)
                            {
                                //TODO: make sure 0-lenght wont blow things up
                                var collection = ((JArray)rel.Value["data"])
                                    .Select(x => new OutResourceObject(x).Instance)
                                    .ToList();

                                var genericType = new OutResourceObject(((JArray)rel.Value["data"])[0]).ObjectType;

                                var listType = typeof(List<>);
                                var constructedListType = listType.MakeGenericType(genericType);

                                var instance = (IList)Activator.CreateInstance(constructedListType);
                                collection.ForEach(x => instance.Add(x));
                                
                                if (prop.PropertyType.GetInterface("IEnumerable") != null)
                                {
                                    prop.SetValue(resourceObject.Instance, instance, null);
                                }
                                else
                                {
                                    throw new CollectionTypeNotSupported();
                                }
                            }
                            else if (prop.PropertyType.IsValueType)
                            {
                                prop.SetValue(resourceObject.Instance, Convert.ChangeType(attributes[propName].ToString(), prop.PropertyType));
                            }
                            else
                            {
                                var relResourceObject = new OutResourceObject(rel.Value["data"]);
                                prop.SetValue(resourceObject.Instance, relResourceObject.Instance);
                            }
                        }
                    };
                });
            }

            /*foreach (var rel in relationships)
            {
                //var relationShipDataNode = rel["data"];
                
                //var relType = relationShipDataNode[]
            }*/

            return resourceObject.Instance as T;
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
                include.Split(',').ToList().ForEach(x =>
                {
                    obj.GetType()
                        .GetProperties()
                        .Where(prop => prop.Name.ToLower() == x)
                        .ToList()
                        .ForEach(prop =>
                        {
                             includes.AddRange(_propertySerializationContext.SerializeFull(obj, prop));
                            
                        });
                });

                return includes;
            }
        }

  
    }
}
