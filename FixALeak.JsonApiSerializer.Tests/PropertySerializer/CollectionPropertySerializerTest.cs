using System;
using System.Collections.Generic;
using FixALeak.JsonApiSerializer.PropertySerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer.Tests.PropertySerializer
{
    [TestClass]
    public class CollectionPropertySerializerTest
    {
        CollectionPropertySerializer sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = new CollectionPropertySerializer(new SingleObjectSerializer());
        }
        [TestMethod]
        public void Serialize_ICollection_When_Given_Valid_Object_Should_Resturn_Serialized()
        {
            var obj = new MainObject()
            {
                ID = 2,
                Name = "abc",
                RelOb = new RelatedObject() { ID = 12, Name = "xyz" },
                RealtedObject2List = new HashSet<RelatedObject2>()
                {
                    new RelatedObject2()
                    {
                        ID = 25,
                        Name = "xz1"
                    },
                     new RelatedObject2()
                    {
                        ID = 26,
                        Name = "xz3"
                    }
                }
            };
            var result = sut.Serialize(obj, obj.GetType().GetProperty("RealtedObject2List"));

            Assert.AreEqual("realtedobject2list", result.Name);

            var data = result.Value.First;
            var links = data.Next;

            var array = new JArray();
            array.Add(JObject.FromObject(new {
                id = 25,
                type = "relatedobject2"
            }));
            array.Add(JObject.FromObject(new
            {
                id = 26,
                type = "relatedobject2"
            }));

            var expectedData = new JProperty("data", array);
            Assert.IsTrue(JToken.DeepEquals(expectedData, data));
        }

        [TestMethod]
        public void Serialize_IEnumerable_When_Given_Valid_Object_Should_Resturn_Serialized()
        {
            var obj = new MainObject()
            {
                ID = 2,
                Name = "abc",
                RelOb = new RelatedObject() { ID = 12, Name = "xyz" },
                RealtedObject3List = new List<RelatedObject3>()
                {
                    new RelatedObject3()
                    {
                        ID = 25,
                        Name = "xz1"
                    },
                     new RelatedObject3()
                    {
                        ID = 26,
                        Name = "xz3"
                    }
                }
            };
            var result = sut.Serialize(obj, obj.GetType().GetProperty("RealtedObject3List"));

            Assert.AreEqual("realtedobject3list", result.Name);

            var data = result.Value.First;
            var links = data.Next;

            var array = new JArray();
            array.Add(JObject.FromObject(new
            {
                id = 25,
                type = "relatedobject3"
            }));
            array.Add(JObject.FromObject(new
            {
                id = 26,
                type = "relatedobject3"
            }));

            var expectedData = new JProperty("data", array);
            Assert.IsTrue(JToken.DeepEquals(expectedData, data));
        }

        [TestMethod]
        public void Serialize_IEnumerable_When_Given_Partial_Object_Should_Resturn_Serialized()
        {
            var obj = new MainObject()
            {
                ID = 2,
                Name = "abc",
                RelOb = new RelatedObject() { ID = 12, Name = "xyz" },
                RealtedObject3List = null
            };
            var result = sut.Serialize(obj, obj.GetType().GetProperty("RealtedObject3List"));

            Assert.AreEqual("realtedobject3list", result.Name);

            var data = result.Value.First;
            var links = data.Next;

            var array = new JArray();
            
            var expectedData = new JProperty("data", array);
            Assert.IsTrue(JToken.DeepEquals(expectedData, data));
        }

        [TestMethod]
        public void SerializeFull_IEnumerable_When_Given_Valid_Object_Should_Resturn_Serialized()
        {
            var obj = new MainObject()
            {
                ID = 2,
                Name = "abc",
                RelOb = new RelatedObject() { ID = 12, Name = "xyz" },
                RealtedObject3List = new List<RelatedObject3>()
                {
                    new RelatedObject3()
                    {
                        ID = 25,
                        Name = "xz1"
                    },
                     new RelatedObject3()
                    {
                        ID = 26,
                        Name = "xz3"
                    }
                }
            };
            var result = sut.SerializeFull(obj, obj.GetType().GetProperty("RealtedObject3List"));
            var listResult = new List<JObject>(result);

            Assert.AreEqual(2, listResult.Count);

            var expected = new List<JObject>();
            expected.Add(JObject.FromObject(new
            {
                id = 25,
                type = "relatedobject3",
                attributes = new
                {
                    name = "xz1"
                }
            }));
            expected.Add(JObject.FromObject(new
            {
                id = 26,
                type = "relatedobject3",
                attributes = new
                {
                    name = "xz3"
                }
            }));

            Assert.IsTrue(JToken.DeepEquals(expected[0], listResult[0]));
            Assert.IsTrue(JToken.DeepEquals(expected[1], listResult[1]));
        }

        [TestMethod]
        public void SerializeFull_ICollection_When_Given_Valid_Object_Should_Resturn_Serialized()
        {
            var obj = new MainObject()
            {
                ID = 2,
                Name = "abc",
                RelOb = new RelatedObject() { ID = 12, Name = "xyz" },
                RealtedObject2List = new HashSet<RelatedObject2>()
                {
                    new RelatedObject2()
                    {
                        ID = 25,
                        Name = "xz1"
                    },
                     new RelatedObject2()
                    {
                        ID = 26,
                        Name = "xz3"
                    }
                }
            };
            var result = sut.SerializeFull(obj, obj.GetType().GetProperty("RealtedObject2List"));
            var listResult = new List<JObject>(result);

            Assert.AreEqual(2, listResult.Count);

            var expected = new List<JObject>();
            expected.Add(JObject.FromObject(new
            {
                id = 25,
                type = "relatedobject2",
                attributes = new
                {
                    name = "xz1"
                }
            }));
            expected.Add(JObject.FromObject(new
            {
                id = 26,
                type = "relatedobject2",
                attributes = new
                {
                    name = "xz3"
                }
            }));

            Assert.IsTrue(JToken.DeepEquals(expected[0], listResult[0]));
            Assert.IsTrue(JToken.DeepEquals(expected[1], listResult[1]));
        }
    }
}
