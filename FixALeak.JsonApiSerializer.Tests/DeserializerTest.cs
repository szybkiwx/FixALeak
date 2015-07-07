using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixALeak.JsonApiSerializer.Tests
{
    [TestClass]
    public class DeserializerTest
    {
        Serializer sut;

        [TestInitialize]
        public void SetUp()
        {
            sut = new Serializer(null, null);
        }

        [TestMethod]
        public void Serializer_Deserialize()
        {
            string value = @"{
              ""data"": {
                ""type"": ""deserializertestobject"",
                ""attributes"": {
                  ""title"": ""Ember Hamster"",
                  ""src"": ""http://example.com/images/productivity.png"",
                  ""somefloat"": 4.5,
                  ""somedecimal"": 4.5,  
                   ""somedouble"": 4.5
                },
                ""relationships"": {
                  ""des"": {
                    ""data"": { ""type"": ""deserializertestrelatedobject"", ""id"": 9 }
                  },
                  ""descollection"": {
                    ""data"": [
                        { ""type"": ""deserializertestrelatedobject"", ""id"": 20 },
                        { ""type"": ""deserializertestrelatedobject"", ""id"": 21 }
                    ]
                  }
                }
              }
            }";
            var result = sut.Deserialize<DeserializerTestObject>(value);
        }
    }
}
