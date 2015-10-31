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
                //return Ok(SerializerBuilder.Create().Serialize(categories, include));
                return Ok(categories);
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
            var userId = Guid.Parse(user.Identity.GetUserId());
            category.UserId = userId;
            if (_categoryService.Exists(userId, category.Name))
            {
                var responseError = new HttpResponseMessage(HttpStatusCode.Conflict);
                responseError.Content = new StringContent("Category exixsts");
                return ResponseMessage(responseError); 
            }
            var created = _categoryService.AddCategory(category);
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new StringContent(serializer.Serialize(created).ToString());
            return ResponseMessage(response);
        }


        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult Get(int id, IPrincipal user)
        {

            var serializer = SerializerBuilder.Create();
            var userId = Guid.Parse(user.Identity.GetUserId());
            var result = _categoryService.GetCategories(userId).FirstOrDefault(cat => cat.ID == id);
            if(result == null)
            {
                var responseError = new HttpResponseMessage(HttpStatusCode.NotFound);
                responseError.Content = new StringContent("Category does not exixst");
                return ResponseMessage(responseError);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(result).ToString());
            return ResponseMessage(response);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id, IPrincipal user)
        {

            var result = _categoryService.Remove(id);

            return Ok();

        }


    }
}
