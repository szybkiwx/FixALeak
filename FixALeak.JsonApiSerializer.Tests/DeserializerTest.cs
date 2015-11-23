using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixALeak.JsonApiSerializer.PropertyDeserializer;

namespace FixALeak.JsonApiSerializer.Tests
{
    [TestClass]
    public class DeserializerTest
    {
        Serializer sut;

        [TestInitialize]
        public void SetUp()
        {
            var aggregate = new PropertyDeserilaizerAggregate() {
                CollectionPropertySerializer = new CollectionPropertyDeserializer(),
                RefPropertyDeserializer = new RefPropertyDeserializer(),
                ValuePropertySerializer = new ValuePropertyDeserializer()
            };
            
            sut = new Serializer(null, new PorpertyDeserialziationContext(aggregate), null);
        }

        [TestMethod]
        public void Deserializer_Deserialize()
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
                  ""deserializertestrelatedobject"": {
                    ""data"": { ""type"": ""deserializertestrelatedobjects"", ""id"": 9 }
                  },
                  ""descollection"": {
                    ""data"": [
                        { ""type"": ""deserializertestrelatedobjects"", ""id"": 20 },
                        { ""type"": ""deserializertestrelatedobjects"", ""id"": 21 }
                    ]
                  },
                  ""descollection2"": {
                    ""data"": [
                        { ""type"": ""deserializertestrelatedobjects"", ""id"": 20 },
                        { ""type"": ""deserializertestrelatedobjects"", ""id"": 21 }
                    ]
                  },
                  ""descollection3"": {
                    ""data"": [
                        { ""type"": ""deserializertestrelatedobject"", ""id"": 20 },
                        { ""type"": ""deserializertestrelatedobject"", ""id"": 21 }
                    ]
                  }
                }
              }
            }";

            var coll = new List<DeserializerTestRelatedObject>()
                {
                    new DeserializerTestRelatedObject()
                    {
                        ID=20
                    },
                    new DeserializerTestRelatedObject()
                    {
                        ID=21
                    }
                };

            var expected = new DeserializerTestObject()
            {
                Title = "Ember Hamster",
                Src = "http://example.com/images/productivity.png",
                SomeFloat = 4.5f,
                SomeDecimal = 4.5m,
                SomeDouble = 4.5d,
                DeserializerTestRelatedObject = new DeserializerTestRelatedObject()
                {
                    ID = 9
                },
                DeserializerTestRelatedObjectID = 9,
                DesCollection = coll,
                DesCollection2 = coll,
                DesCollection3 = coll
            };
            var result = sut.Deserialize<DeserializerTestObject>(value);
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        [ExpectedException(typeof(MalformedJsonApiDocumentException))]
        public void Deserializer_Deserialize_MissingData_ThrowException1()
        {
            string value = @"{
              ""datsra"": {
                ""type"": ""deserializertestobject"",
                ""attributes"": {
                  ""title"": ""Die Erste Welt Probleme"",
                  ""somefloat"": 4.5,

                }
              }
            }";

            sut.Deserialize<DeserializerTestObject>(value);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedJsonApiDocumentException))]
        public void Deserializer_Deserialize_MissingData_ThrowException2()
        {
            string value = @"{
              ""Data"": {
                ""type"": ""deserializertestobject"",
                ""attributes"": {
                  ""title"": ""Die Erste Welt Probleme"",
                  ""somefloat"": 4.5,

                }
              }
            }";

            sut.Deserialize<DeserializerTestObject>(value);
        }


        [TestMethod]
        public void Deserializer_DeserializePatchGeneric()
        {
            string value = @"{
              ""data"": {
                ""type"": ""deserializertestobject"",
                ""attributes"": {
                  ""title"": ""Die Erste Welt Probleme"",
                  ""somefloat"": 4.5,

                }
              }
            }";

            var coll = new List<DeserializerTestRelatedObject>()
                {
                    new DeserializerTestRelatedObject()
                    {
                        ID=20
                    },
                    new DeserializerTestRelatedObject()
                    {
                        ID=21
                    }
                };

            var original = new DeserializerTestObject()
            {
                Title = "Ember Hamster",
                Src = "http://example.com/images/productivity.png",
                SomeFloat = 1.5f,
                SomeDecimal = 1.5m,
                SomeDouble = 1.5d,
                DeserializerTestRelatedObject = new DeserializerTestRelatedObject()
                {
                    ID = 9
                },
                DeserializerTestRelatedObjectID = 9,
                DesCollection = coll,
                DesCollection2 = coll,
                DesCollection3 = coll
            };

            var expected = new DeserializerTestObject()
            {
                Title = "Die Erste Welt Probleme",
                Src = "http://example.com/images/productivity.png",
                SomeFloat = 4.5f,
                SomeDecimal = 1.5m,
                SomeDouble = 1.5d,
                DeserializerTestRelatedObject = new DeserializerTestRelatedObject()
                {
                    ID = 9
                },
                DeserializerTestRelatedObjectID = 9,
                DesCollection = coll,
                DesCollection2 = coll,
                DesCollection3 = coll
            };
            var patch = sut.DeserializePatch<DeserializerTestObject>(value);

            patch.Patch(original);

            Assert.AreEqual(expected, original);
        }

        [TestMethod]
        public void Deserializer_DeserializePatch()
        {
            string value = @"{
              ""data"": {
                ""type"": ""deserializertestobject"",
                ""attributes"": {
                  ""title"": ""Die Erste Welt Probleme"",
                  ""somefloat"": 4.5,

                }
              }
            }";

            var coll = new List<DeserializerTestRelatedObject>()
                {
                    new DeserializerTestRelatedObject()
                    {
                        ID=20
                    },
                    new DeserializerTestRelatedObject()
                    {
                        ID=21
                    }
                };

            var original = new DeserializerTestObject()
            {
                Title = "Ember Hamster",
                Src = "http://example.com/images/productivity.png",
                SomeFloat = 1.5f,
                SomeDecimal = 1.5m,
                SomeDouble = 1.5d,
                DeserializerTestRelatedObject = new DeserializerTestRelatedObject()
                {
                    ID = 9
                },
                DeserializerTestRelatedObjectID = 9,
                DesCollection = coll,
                DesCollection2 = coll,
                DesCollection3 = coll
            };

            var expected = new DeserializerTestObject()
            {
                Title = "Die Erste Welt Probleme",
                Src = "http://example.com/images/productivity.png",
                SomeFloat = 4.5f,
                SomeDecimal = 1.5m,
                SomeDouble = 1.5d,
                DeserializerTestRelatedObject = new DeserializerTestRelatedObject()
                {
                    ID = 9
                },
                DeserializerTestRelatedObjectID = 9,
                DesCollection = coll,
                DesCollection2 = coll,
                DesCollection3 = coll
            };
            var result = sut.DeserializePatch(value, typeof(DeserializerTestObject));

            Assert.IsInstanceOfType(result, typeof(JsonApiPatch<DeserializerTestObject>));

            var patch = (JsonApiPatch<DeserializerTestObject>)result;

            patch.Patch(original);

            Assert.AreEqual(expected, original);
        }

        [TestMethod]
        [ExpectedException(typeof(RelationshipUpdateForbiddenException))]
        public void Deserializer_DeserializePatchGeneric_WhenRelationshipChanged_ThrowException()
        {
            string value = @"{
              ""data"": {
                ""type"": ""deserializertestobject"",
                ""attributes"": {
                  ""title"": ""Die Erste Welt Probleme"",
                  ""somefloat"": 4.5,

                },
                ""relationships"": {
                  ""deserializertestrelatedobject"": {
                    ""data"": { ""type"": ""deserializertestrelatedobjects"", ""id"": 19 }
                  }
                }
                
              }
            }";

            var coll = new List<DeserializerTestRelatedObject>()
                {
                    new DeserializerTestRelatedObject()
                    {
                        ID=20
                    },
                    new DeserializerTestRelatedObject()
                    {
                        ID=21
                    }
                };

            var original = new DeserializerTestObject()
            {
                Title = "Ember Hamster",
                Src = "http://example.com/images/productivity.png",
                SomeFloat = 1.5f,
                SomeDecimal = 1.5m,
                SomeDouble = 1.5d,
                DeserializerTestRelatedObject = new DeserializerTestRelatedObject()
                {
                    ID = 9
                },
                DeserializerTestRelatedObjectID = 9,
                DesCollection = coll,
                DesCollection2 = coll,
                DesCollection3 = coll
            };

          
            var patch = sut.DeserializePatch<DeserializerTestObject>(value);
        
        }


    }
}
