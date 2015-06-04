using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixALeak.JsonApiSerializer.Tests
{
    /// <summary>
    /// Summary description for UrlBuilder
    /// </summary>
    [TestClass]
    public class UrlBuilderTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            SerializerConfiguration.Prefix = "asdf";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            SerializerConfiguration.Prefix = string.Empty;
        }
        public TestContext TestContext {get;set;}

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion



        [TestMethod]
        public void Test_Resource_Id_Chain()
        {
            string result = UrlBuilder.Initialize().Resource("leaf").Id(12).ToString();
            Assert.AreEqual("/asdf/leaves/12", result);
        }

        [TestMethod]
        public void Test_Resource_Id_Chain_2()
        {
            string result = UrlBuilder.Initialize().Resource("leaf").Id(12).Resource("x").Resource("what").ToString();
            Assert.AreEqual("/asdf/leaves/12/x/what", result);
        }

        [TestMethod]
        public void Test_No_Prefix()
        {
            SerializerConfiguration.Prefix = string.Empty;
            string result = UrlBuilder.Initialize().Resource("leaf").Id(12).ToString();
            Assert.AreEqual("/leaves/12", result);
        }
    }
}
