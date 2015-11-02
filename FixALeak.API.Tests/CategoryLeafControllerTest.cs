using FixALeak.API.Controllers;
using FixALeak.Data.Entities;
using FixALeak.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace FixALeak.API.Tests
{
    [TestClass]
    public class CategoryLeafControllerTest
    {
        private Mock<ICategoryLeafService> _categoryLeafService;
        private Mock<ICategoryService> _categoryService;
        private Mock<UrlHelper> _urlHelper;

        private CategoryLeafController sut;

        [TestInitialize]
        public void SetUp()
        {
            _urlHelper = new Mock<UrlHelper>();
            _categoryLeafService = new Mock<ICategoryLeafService>();
            _categoryService = new Mock<ICategoryService>();
            sut = new CategoryLeafController(_categoryLeafService.Object, _categoryService.Object);
            sut.Url = _urlHelper.Object;

        }

        [TestMethod]
        public void TestAdd_WhenCategoryLeafDoesntExist_CategoryLeafeAdded()
        {
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



            CategoryLeaf expectedCatLeaf = new CategoryLeaf()
            {
                ID = 12,
                Name = "test upload",
                CategoryID = 5
            };

            _categoryLeafService
                .Setup(x => x.Add(It.IsAny<CategoryLeaf>()))
                .Returns((CategoryLeaf c) => { c.ID = 12; return c; });



            var userId = Guid.NewGuid();


            _urlHelper.Setup(x => x.Link("GetCategoryLeaf", It.IsAny<object>())).Returns("http://localhost/api/categoryleaves/12");
            _categoryService.Setup(x => x.Get(5)).Returns(new Category() { });

            var response = sut.Add(cat, FakeUserFactory.NewFakeUser(userId, ""));

            _categoryLeafService.Verify(x => x.Add(It.IsAny<CategoryLeaf>()));

            Assert.AreEqual(5, expectedCatLeaf.CategoryID);

            Assert.IsInstanceOfType(response, typeof(CreatedNegotiatedContentResult<CategoryLeaf>));
        }

        [TestMethod]
        public void TestAdd_WhenCategoryLeafDoesntExistAndCategoryDoesntExist_CategoryLeafeAdded()
        {
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



            CategoryLeaf expectedCatLeaf = new CategoryLeaf()
            {
                ID = 12,
                Name = "test upload",
                CategoryID = 5
            };

            _categoryLeafService
                .Setup(x => x.Add(It.IsAny<CategoryLeaf>()))
                .Returns((CategoryLeaf c) => { c.ID = 12; return c; });



            var userId = Guid.NewGuid();


            _urlHelper.Setup(x => x.Link("GetCategoryLeaf", It.IsAny<object>())).Returns("http://localhost/api/categoryleaves/12");
            _categoryService.Setup(x => x.Get(5)).Returns<Category>(null);

            var response = sut.Add(cat, FakeUserFactory.NewFakeUser(userId, ""));

            _categoryLeafService.Verify(x => x.Add(It.IsAny<CategoryLeaf>()), Times.Never());

            Assert.AreEqual(5, expectedCatLeaf.CategoryID);

            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void TestAdd_WhenCategorLeafyExists_ConfilctReturned()
        {
            var userId = Guid.NewGuid();

            _categoryLeafService.Setup(x => x.Exists(5, "test upload")).Returns(true);
            CategoryLeaf cat = new CategoryLeaf()
            {
                Name = "test upload",
                CategoryID = 5
            };
            var response = sut.Add(cat, FakeUserFactory.NewFakeUser(userId, ""));
            Assert.IsInstanceOfType(response, typeof(ConflictResult));
        }
        

        [TestMethod]
        public void TestGet_WhenCategoryLeafExists_CategoryLeafReturned()
        {

            var userId = Guid.NewGuid();
            var expected = new CategoryLeaf()
            {

                ID = 12,
                Name = "xyz",
                CategoryID = 5,
                Expenses = new List<Expense>
                {
                    new Expense()
                    {
                        Title = "abc",
                        Value = 12m,
                        ID = 18
                    }
                }
            };
            _categoryLeafService.Setup(x => x.Get(12)).Returns(expected);
            var response = sut.Get(12, FakeUserFactory.NewFakeUser(userId, ""), "");
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<CategoryLeaf>));
            Assert.AreEqual(expected, ((OkNegotiatedContentResult<CategoryLeaf>)response).Content);
        }

        [TestMethod]
        public void TestGetWithInclude_WhenCategoryLeafExists_CategoryLeafReturned()
        {

            var userId = Guid.NewGuid();
            var expected = new CategoryLeaf()
            {

                ID = 12,
                Name = "xyz",
                CategoryID = 5,
                Expenses = new List<Expense>
                {
                    new Expense()
                    {
                        Title = "abc",
                        Value = 12m,
                        ID = 18
                    }
                }
            };
            _categoryLeafService.Setup(x => x.GetWithIncludes(12, It.IsAny<string>())).Returns(expected);

            var response = sut.Get(12, FakeUserFactory.NewFakeUser(userId, ""), "expenses");
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<CategoryLeaf>));
            var result = (OkNegotiatedContentResult<CategoryLeaf>)response;
            Assert.AreEqual(expected, result.Content);
        }
    }
}
