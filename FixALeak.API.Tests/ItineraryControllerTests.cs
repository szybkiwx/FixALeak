using FixALeak.API.Controllers;
using FixALeak.Data.Entities;
using FixALeak.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace FixALeak.API.Tests
{
    [TestClass]
    public class  ItineraryControllerTests
    {
        private Mock<IItineraryService> _itineraryService;
        private Mock<UrlHelper> _urlHelper;

        private ItineraryController sut;
        [TestInitialize]
        public void SetUp()
        {
            _itineraryService = new Mock<IItineraryService>();
            _urlHelper = new Mock<UrlHelper>();
            sut = new ItineraryController(_itineraryService.Object);

        }

        [TestMethod]
        public void TestGetItinerary_ItineraryExists_ReturnsOk()
        {
            Guid userId = Guid.NewGuid();

            var testObject = new Itinerary()
            {
                ID = new Random().Next(),
                UserId = userId
            };

            _itineraryService.Setup(x => x.Get(15)).Returns(testObject);
            _itineraryService.Setup(x => x.Exists(15)).Returns(true);

            var response = sut.Get(15, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Itinerary>));
            var returned = ((OkNegotiatedContentResult<Itinerary>)response).Content;
            Assert.AreEqual(testObject, returned);

        }

        [TestMethod]
        public void TestGetItinerary_ItineraryDoesntExist_ReturnsNotFound()
        {
            Guid userId = Guid.NewGuid();

            var testObject = new Itinerary()
            {
                ID = new Random().Next(),
                UserId = userId
            };

            _itineraryService.Setup(x => x.Get(15)).Returns(testObject);
            _itineraryService.Setup(x => x.Exists(15)).Returns(false);

            var response = sut.Get(15, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(NotFoundResult));

        }

        [TestMethod]
        public void TestAddItinerary_ItineraryDoesntExist_ReturnsCreated()
        {
            Guid userId = Guid.NewGuid();

            var testObject = new Itinerary()
            {
                UserId = userId
            };

            var returned = new Itinerary()
            {
                ID = new Random().Next(),
                UserId = userId
            };

            _itineraryService.Setup(x => x.Add(testObject)).Returns(returned);
            _itineraryService.Setup(x => x.Exists(15)).Returns(false);

            _urlHelper.Setup(x => x.Link("GetItinerary", It.IsAny<object>())).Returns("http://locahost/api/itineraries/"+ returned.ID);
            sut.Url = _urlHelper.Object;
            var response = sut.Add(testObject, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(CreatedNegotiatedContentResult<Itinerary>));
            Assert.IsTrue(
                ((CreatedNegotiatedContentResult<Itinerary>)response).Location.AbsoluteUri == "http://locahost/api/itineraries/"+ returned.ID
            );
        }

        [TestMethod]
        public void TestGetExpenses_ItineraryExists_ReturnsNotFound()
        {
            Guid userId = Guid.NewGuid();

            _itineraryService.Setup(x => x.Exists(15)).Returns(false);

            var response = sut.GetExpenses(15, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(NotFoundResult));

        }

        [TestMethod]
        public void TestGetExpenses_ItineraryExists_ReturnsOk()
        {
            Guid userId = Guid.NewGuid();

            _itineraryService.Setup(x => x.Exists(15)).Returns(true);

            var response = sut.GetExpenses(15, FakeUserFactory.NewFakeUser(userId, ""));

            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IEnumerable<Expense>>));

        }

    }
}
