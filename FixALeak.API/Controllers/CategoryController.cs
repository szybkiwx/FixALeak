using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using FixALeak.Service;
using Microsoft.AspNet.Identity;

using FixALeak.Data.Entities;

using System.Security.Principal;


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
        public IHttpActionResult Add(Category category, IPrincipal user)
        {
            var userId = Guid.Parse(user.Identity.GetUserId());
            category.UserId = userId;
            if (_categoryService.Exists(userId, category.Name))
            {
                return Conflict();
            }
            var created = _categoryService.AddCategory(category);
            ;

            return Created(Url.Link("GetCategory", new { id = created.ID }), created);
        } 


        [Route("{id:int}", Name = "GetCategory")]
        [HttpGet]
        public IHttpActionResult Get(int id, IPrincipal user)
        {

            var userId = Guid.Parse(user.Identity.GetUserId());
            var result = _categoryService.Get(id);
            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id, IPrincipal user)
        {

            var result = _categoryService.Remove(id);

            return Ok();

        }

        [Route("{id:int}")]
        [HttpPatch]
        public IHttpActionResult Update(int id, IPrincipal user)
        {

            var result = _categoryService.Remove(id);

            return Ok();

        }

    }
}
