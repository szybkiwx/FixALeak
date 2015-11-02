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
    [RoutePrefix("api/categoryleaves")]
    [Authorize]
    public class CategoryLeafController : ApiController
    {
        private ICategoryLeafService _categoryLeafService;
        private ICategoryService _categoryService;

        public CategoryLeafController(ICategoryLeafService categoryLeafService, ICategoryService categoryService)
        {
            _categoryLeafService = categoryLeafService;
            _categoryService = categoryService;
        }


        [Route("")]
        [HttpPost]
        public IHttpActionResult Add(CategoryLeaf categoryLeaf, IPrincipal user)
        {

            if(_categoryLeafService.Exists(categoryLeaf.CategoryID, categoryLeaf.Name))
            {
                return Conflict();
            }

            if(_categoryService.Get(categoryLeaf.CategoryID) == null)
            {
                return BadRequest("Parent category does not exist");
            }

            var created = _categoryLeafService.Add(categoryLeaf);

            return Created(Url.Link("GetCategoryLeaf", new { id = created.ID }), created);
        }


        [Route("{id:int}", Name="GetCategoryLeaf")]
        [HttpGet]
        public IHttpActionResult Get(int id, IPrincipal user, [FromUri] string include)
        {
            CategoryLeaf result;
            if (include == "expenses")
            {
                result = _categoryLeafService.GetWithIncludes(id, include);
            }
            else
            {
                result = _categoryLeafService.Get(id);
            }

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();

        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id, IPrincipal user)
        {
            if (_categoryLeafService.Get(id) == null)
            {
                return NotFound();
            }
            var result = _categoryLeafService.Remove(id);

            return Ok();

        }
    }
}
