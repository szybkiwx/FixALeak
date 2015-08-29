using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixALeak.JsonApiSerializer.Tests
{
    class TestEntity
    {
        public int ID { get; set; }
    }

    [TestClass]
    public class ResourceObjectTest
    {
        private InResourceObject sut;

        [TestInitialize]
        public void SetUp()
        {
            var entity = new TestEntity();
            entity.ID = 12;
            sut = new InResourceObject(entity);
        }

        [TestMethod]
        public void Test_Get_Id()
        {
            Assert.AreEqual(12, sut.ID);
        }

        [TestMethod]
        public void Test_Get_TypeName()
        {
            Assert.AreEqual("testentity", sut.TypeName);
        }

        [TestMethod]
        public void Test_GetSelfLink()
        {
            Assert.AreEqual("/testentities/12", sut.GetSelfLink().ToString());
        }

        [TestMethod]
        public void Test_GetRelatedSelfLink()
        {
            Assert.AreEqual("/testentities/12/relationships/doors", sut.GetRelatedSelfLink("door").ToString());
        }

        [TestMethod]
        public void Test_GetRelatedLink()
        {
            Assert.AreEqual("/testentities/12/doors", sut.GetRelatedLink("door").ToString());
        }
    }
}
