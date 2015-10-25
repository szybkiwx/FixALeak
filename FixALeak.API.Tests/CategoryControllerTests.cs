using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixALeak.API.Controllers;
using Moq;
using FixALeak.Service;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Security.Principal;
using System.Collections.Generic;
using System.Security.Claims;
using FixALeak.Data.Entities;
using System.Web.Http.Results;
using System.Net;
using Newtonsoft.Json.Linq;

namespace FixALeak.API.Tests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryService;

        private CategoryController sut;

        [TestInitialize]
        public void SetUp()
        {
            _categoryService = new Mock<ICategoryService>();
            sut = new CategoryController(_categoryService.Object);

        }

        [TestMethod]
        public void TestAdd_WhenCategoryDoesntExist_CategoryAdded()
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


            _categoryService.Setup(x => x.GetCategories(userId)).Returns(new List<Category>()
            {
                new Category()
                {
                    Name = "xyz"
                }
            });


            Category cat = new Category()
            {
                Name = "test upload",
                UserId = userId
            };

            _categoryService.Setup(x => x.AddCategory(It.IsAny<Category>())).Returns((Category c) => { c.ID = 12; return c; });

            var response = sut.Add(request, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(ResponseMessageResult));
            Assert.AreEqual(HttpStatusCode.Created, ((ResponseMessageResult)response).Response.StatusCode);
        }

        [TestMethod]
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


        [TestMethod]
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
        }
    }
}
