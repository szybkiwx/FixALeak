using FixALeak.API.Controllers;
using FixALeak.Data.Entities;
using FixALeak.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace FixALeak.API.Tests
{
    [TestClass]
    public class ExpensesControllerTest
    {
        private ExpenseController sut;

        private Mock<IExpenseService> _expenseService;

        private Mock<ICategoryLeafService> _categoryLeafService;

        private Mock<UrlHelper> _urlHelper;

        [TestInitialize]
        public void SetUp()
        {
            _categoryLeafService = new Mock<ICategoryLeafService>();
            _expenseService = new Mock<IExpenseService>();
            _urlHelper = new Mock<UrlHelper>();
            sut = new ExpenseController(_expenseService.Object, _categoryLeafService.Object);
            sut.Url = _urlHelper.Object;
        }

        [TestMethod]
        public void TestGet_DoesntExist_ReturnNotFound()
        {
            int testExpId = 123;
            _expenseService.Setup(x => x.Get(testExpId)).Returns<Expense>(null);
            var result = sut.Get(testExpId);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void TestGet_Exists_ReturnOk()
        {
            int testExpId = 123;
            _expenseService.Setup(x => x.Get(testExpId)).Returns(new Expense() { ID = testExpId });
            var result = sut.Get(testExpId);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Expense>));
        }

        [TestMethod]
        public void TestAdd_ExpenseDoesntExists_AddExpenseAndReturnOk()
        {
            int testExpId = 123;
            _categoryLeafService.Setup(x => x.Get(10)).Returns(new CategoryLeaf() { ID = 10 });
            _expenseService.Setup(x => x.Get(testExpId)).Returns<Expense>(null);

            var exp = new Expense()
            {
                CategoryLeafID = 10,
                Title = "Nowy",
                Value = 12.3m
            };

            var expected = new Expense()
            {
                ID = 12,
                CategoryLeafID = 10,
                Title = "Nowy",
                Value = 12.3m
            };

            _expenseService.Setup(x => x.Add(exp)).Returns(expected);
            _urlHelper.Setup(x => x.Link("GetExpense", It.IsAny<object>())).Returns("http://locahost/api/expenses/12");

            var result = sut.Add(exp);
            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<Expense>));
            Assert.AreEqual(expected, ((CreatedNegotiatedContentResult<Expense>)result).Content);
        }
    }
}
