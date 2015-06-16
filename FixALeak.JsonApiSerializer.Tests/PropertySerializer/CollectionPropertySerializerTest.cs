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
        public void Serialize_When_Given_Valid_Object_Should_Resturn_Serialized()
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

            /*var expectedData = new JProperty("data", new JObject.FromObject(
                
                new List<object>() {
                    new {
                        id = 25,
                        type = "relatedobject2"
                    },
                    new {
                        id = 26,
                        type = "relatedobject2"
                    }
                }));
            Assert.AreEqual(expectedData.ToString(), data.ToString());*/
        }
    }
}
