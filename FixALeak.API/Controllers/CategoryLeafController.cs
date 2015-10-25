using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FixALeak.API.Models;
using FixALeak.Data.Entities;
using FixALeak.Service;
using FixALeak.JsonApiSerializer;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/categories/{cat:int}/categoryleaves")]
    [Authorize]
    public class CategoryLeafController : ApiController
    {
        private ICategoryLeafService _categoryLeafService;

        public CategoryLeafController(ICategoryLeafService categoryLeafService)
        {
            _categoryLeafService = categoryLeafService;
        }


        [Route("")]
        [HttpPost]
        public IHttpActionResult Add(HttpRequestMessage request, int cat, IPrincipal user)
        {

            var serializer = SerializerBuilder.Create();
            var content = request.Content.ReadAsStringAsync().Result;
            var userId = Guid.Parse(user.Identity.GetUserId());
            var categoryLeaf = serializer.Deserialize<CategoryLeaf>(content);

            if(_categoryLeafService.Exists(cat, categoryLeaf.Name))
            {
                var responseError = new HttpResponseMessage(HttpStatusCode.Conflict);
                responseError.Content = new StringContent("Category Leaf exixsts");
                return ResponseMessage(responseError);
            }

            categoryLeaf.CategoryID = cat;
            _categoryLeafService.Add(categoryLeaf);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new StringContent("Category Leaf created");
            return ResponseMessage(response);
        }


        [Route("/{id:int}")]
        [HttpGet]
        public IHttpActionResult Get(int id, IPrincipal user, [FromUri] string include)
        {
            var serializer = SerializerBuilder.Create();

            var result =_categoryLeafService.Get(id);
            if (result != null)
            {
                return Ok(SerializerBuilder.Create().Serialize(serializer, include));
            }

            return NotFound();

        }

        [Route("/{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id, IPrincipal user)
        {
            
            var result = _categoryLeafService.Remove(id);

            return Ok();

        }
    }
}
