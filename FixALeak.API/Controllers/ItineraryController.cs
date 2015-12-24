using FixALeak.Data.Entities;
using FixALeak.Service;
using System;
using System.Security.Principal;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/itineraries")]
    [Authorize]

    public class ItineraryController : ApiController
    {
        private IItineraryService _itineraryService;

        public ItineraryController(IItineraryService itineraryService)
        {
            _itineraryService = itineraryService;
        }


        [Route("{id:int}", Name = "GetItinerary")]
        [HttpGet]
        public IHttpActionResult Get(int id, IPrincipal user)
        {
            if (_itineraryService.Exists(id))
            {
                return Ok(_itineraryService.Get(id));
            }
            return NotFound();
        }


        [Route("", Name = "AddItinerary")]
        [HttpPost]
        public IHttpActionResult Add(Itinerary itinerary, IPrincipal principal)
        {
            itinerary.UserId = Guid.Parse(principal.Identity.GetUserId());


            var result =  _itineraryService.Add(itinerary);
            return Created(Url.Link("GetItinerary", new { id = result.ID }), result);
        }

        [Route("{id:int}/relationships/expenses", Name = "GetItineraryExpenses")]
        [HttpGet]
        public IHttpActionResult GetExpenses(int id, IPrincipal principal)
        {
            if(!_itineraryService.Exists(id))
            {
                return NotFound(); 
            }

            return Ok(_itineraryService.GetExpenses(id));


        }
    }
}

