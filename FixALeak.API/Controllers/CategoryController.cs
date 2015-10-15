using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FixALeak.API.Models;
using FixALeak.Service;
using Microsoft.AspNet.Identity;
using AutoMapper;
using FixALeak.Data.Entities;
using FixALeak.JsonApiSerializer;
using FixALeak.API.Extensions;
using System.Security.Principal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/categories")]
    [Authorize]
    public class CategoryController : ApiController
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService catService) 
        {
            _categoryService = catService;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetCategories([FromUri] string include, IPrincipal user) 
        {

            try
            {
                IEnumerable<Category> categories = _categoryService.GetCategoryTree(Guid.Parse(user.Identity.GetUserId())).ToList();
                return Ok(SerializerBuilder.Create().Serialize(categories, include));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Add(HttpRequestMessage request, IPrincipal user)
        {
            var serializer = SerializerBuilder.Create();

            var content = request.Content.ReadAsStringAsync().Result;


            Category category = serializer.Deserialize<Category>(content);
            category.UserId = Guid.Parse(user.Identity.GetUserId());
            var created = _categoryService.AddCategory(category);
            return Ok(serializer.Serialize(created));
            //return Ok();
        }





    }
}
