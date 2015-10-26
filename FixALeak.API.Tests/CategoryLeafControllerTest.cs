using FixALeak.API.Controllers;
using FixALeak.Data.Entities;
using FixALeak.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace FixALeak.API.Tests
{
    [TestClass]
    public class CategoryLeafControllerTest
    {
        private Mock<ICategoryLeafService> _categoryLeafService;

        private CategoryLeafController sut;

        [TestInitialize]
        public void SetUp()
        {
            _categoryLeafService = new Mock<ICategoryLeafService>();
            sut = new CategoryLeafController(_categoryLeafService.Object);

        }

        [TestMethod]
        public void TestAdd_WhenCategoryLeafDoesntExist_CategoryLeafeAdded()
        {
            var postData = @"{
                ""data"": {
                    ""type"": ""categoryleaves"",
                    ""attributes"": {
                        ""name"": ""test upload""
                    },
                    ""relationships"" : {
                        ""category"": {
                            ""type"" : ""categories"", 
                            ""id"" : 5
                        }
                    }
                }
            }";


            var request = new RequestMessageBuilder()
                .WithAbsoluteUrl("http://localhost/api/categories/12/categoryleavesy")
                .WithMethod(HttpMethod.Post)
                .WithBody(postData)
                .Build();


            _categoryLeafService.Setup(x => x.GetCategoryLeaves(5)).Returns(new List<CategoryLeaf>()
            {
                new CategoryLeaf()
                {
                    Name = "xyz"
                }
            });


            CategoryLeaf cat = new CategoryLeaf()
            {
                Name = "test upload",
                CategoryID = 5
            };

            CategoryLeaf expectedCatLeaf = null;

            _categoryLeafService
                .Setup(x => x.Add(It.IsAny<CategoryLeaf>()))
                .Callback((CategoryLeaf c) => { expectedCatLeaf = c; })
                .Returns((CategoryLeaf c) => { c.ID = 12; return c; });



            var userId = Guid.NewGuid();

            var response = sut.Add(request, 5, FakeUserFactory.NewFakeUser(userId, ""));

            _categoryLeafService.Verify(x => x.Add(It.IsAny<CategoryLeaf>()));

            Assert.AreEqual(5, expectedCatLeaf.CategoryID);

            Assert.IsInstanceOfType(response, typeof(ResponseMessageResult));
            Assert.AreEqual(HttpStatusCode.Created, ((ResponseMessageResult)response).Response.StatusCode);
        }

       /* [TestMethod]
        public void TestAdd_WhenCategoryExists_CategoryAdded()
        {
            var postData = @"{
                ""data"": {
                    ""type"": ""categories"",
                    ""attributes"": {
                        ""name"": ""test upload""
                    }
                }
            }";

            var userId = Guid.NewGuid();

            var request = new RequestMessageBuilder()
                .WithAbsoluteUrl("http://localhost/api/category")
                .WithMethod(HttpMethod.Post)
                .WithBody(postData)
                .Build();


           
            _categoryService.Setup(x => x.Exists(userId, "test upload")).Returns(true);

            var response = sut.Add(request, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(ResponseMessageResult));
            Assert.AreEqual(HttpStatusCode.Conflict, ((ResponseMessageResult)response).Response.StatusCode);
        }
        */

       /* [TestMethod]
        public void TestGet_WhenCategoryExists_CategoryReturned()
        {

            var userId = Guid.NewGuid();

            _categoryService.Setup(x => x.GetCategories(userId)).Returns(new List<Category>()
            {
                new Category()
                {
                    ID = 12,
                    Name = "xyz",
                    UserId = userId
                }
            });

 
            var response = sut.Get(12, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(ResponseMessageResult));
            var result = ((ResponseMessageResult)response).Response;
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var content = result.Content.ReadAsStringAsync().Result;

            var root = JToken.Parse(content);
            var data = root["data"];
            Assert.AreEqual("category", data["type"]);
            Assert.AreEqual(12, data["id"]);

            var attributes = data["attributes"];
            Assert.AreEqual("xyz", attributes["name"]);
            Assert.AreEqual(userId.ToString(), attributes["userid"]);
        }*/
    }
}
