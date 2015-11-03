using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer.Tests
{
    [TestClass]
    public class JsonApiPatchTest
    {

        class TestRelatedObject
        {
            public int ID { get; set; }
            public string Name { get; set; }

        }
        class TestObject
        {
            public int ID { get; set; }
            public int TestRelatedObjectID { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
            public string Foo { get; set; }

            public override bool Equals(object obj)
            {
                var other = (TestObject)obj;
                return ID == other.ID &&
                        TestRelatedObjectID == other.TestRelatedObjectID &&
                        Name == other.Name &&
                        Value == other.Value &&
                        Foo == other.Foo;

            }
        }


        [TestMethod]
        public void TestPatch()
        {
            var test = new TestObject()
            {
                ID = 12,
                Name = "xyz",
                Value = 122.5m,
                Foo = "Bar",
                TestRelatedObjectID = 11
            };

            var expected = new TestObject()
            {
                ID = 12,
                Name = "abc",
                Value = 144.6m,
                Foo = "Bar",
                TestRelatedObjectID = 7

            }; 

            var patch = new JsonApiPatch<TestObject>();
            patch.SetValue("name", "abc");
            patch.SetValue("value", 144.6m);
            patch.SetValue("testrelatedobjectid", 7);

            patch.Patch(test);

            Assert.AreEqual(expected, test);


        }
       
    }
}
