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
using System.Web.Http.Routing;

namespace FixALeak.API.Tests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryService;
        private Mock<UrlHelper> _urlHelper;


        private CategoryController sut;

        [TestInitialize]
        public void SetUp()
        {
            _categoryService = new Mock<ICategoryService>();
            _urlHelper = new Mock<UrlHelper>();
            sut = new CategoryController(_categoryService.Object);

        }

        [TestMethod]
        public void TestAdd_WhenCategoryDoesntExist_CategoryAdded()
        {
            var userId = Guid.NewGuid();

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

            Category created = new Category()
            {
                ID = 12,
                Name = "test upload",
                UserId = userId
            };

            _categoryService.Setup(x => x.AddCategory(It.IsAny<Category>())).Returns((Category c) => { return created; });
            _urlHelper.Setup(x => x.Link("GetCategory", It.IsAny<object>())).Returns("http://locahost/api/categories/12");
            sut.Url = _urlHelper.Object;
            var response = sut.Add(cat, FakeUserFactory.NewFakeUser(userId, ""));
            
            Assert.IsInstanceOfType(response, typeof(CreatedNegotiatedContentResult<Category>));
        }

        [TestMethod]
        public void TestAdd_WhenCategoryExists_CategoryNotAdded()
        {
            var userId = Guid.NewGuid();

            _categoryService.Setup(x => x.Exists(userId, "test upload")).Returns(true);

            Category cat = new Category()
            {
                Name = "test upload"
            };

            var response = sut.Add(cat, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(ConflictResult));
        }


        [TestMethod]
        public void TestGet_WhenCategoryExists_CategoryReturned()
        {

            var userId = Guid.NewGuid();
            var expected = new Category()
            {
                ID = 12,
                Name = "xyz",
                UserId = userId
            };
            _categoryService.Setup(x => x.Get(12)).Returns(
                expected
            );
            var response = sut.Get(12, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Category>));
            Assert.AreEqual(expected, ((OkNegotiatedContentResult<Category>)response).Content);

        }
    }
}
