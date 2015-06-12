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
        public IHttpActionResult GetCategories([FromUri] string include = "") 
        {
            try
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                IEnumerable<Category> categories = _categoryService.GetCategoryTree(userId).ToList();
                
                return Ok(SerializerBuilder.Create().Serialize(categories, include));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        /*[Route("")]
        [HttpPost]
        public IHttpActionResult Add(CategoryVM model)
        {
            Mapper.CreateMap<CategoryVM, Category>();
            Mapper.CreateMap<Category, CategoryVM>();
            var entity = Mapper.Map<CategoryVM, Category>(model);
            entity.UserId = Guid.Parse(User.Identity.GetUserId());
            var created = _categoryService.AddCategory(entity);
            return Ok(Mapper.Map<Category, CategoryVM>(created));
        }*/

        



    }
}
