using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FixALeak.JsonApiSerializer.PropertySerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer.Tests.PropertySerializer
{
    [TestClass]
    public class ValuePropertySerializerTest
    {
        private ValuePropertySerializer sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = new ValuePropertySerializer(new SingleObjectSerializer());
        }

        [TestMethod]
        public void Serialize_Value_When_Given_Valid_Object_Should_Resturn_Serialized()
        {
            var obj = new MainObject()
            {
                ID = 2,
                Name = "abc",
                RelOb = new RelatedObject() { ID = 12, Name = "xyz" },

            };

            var result = sut.Serialize(obj, obj.GetType().GetProperty("RelOb"));
            Assert.AreEqual("relob", result.Name);

            var data = result.Value.First;
            var links = data.Next;

            var expectedData = new JProperty("data",  JObject.FromObject(new {
                id=12,
                type="relatedobject"
            }));
            Assert.AreEqual(expectedData.ToString(), data.ToString());
        }
    }
}
