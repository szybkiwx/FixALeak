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

namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/categories")]
    //[Authorize]
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
                
                var data = categories.Select(x => new {
                    type = "categories",
                    id = x.ID,
                    attributes = new {
                        name = x.Name,
                    },
                    relationships = new {
                        self = "/api/categories/" + x.ID,
                        sub_categories =  x.SubCategories.Select(sub => new {
                            self = "/api/categories/" + x.ID + "/categoryleafs",
                            linkage = new {
                                type = "category_leafs",
                                id = sub.ID
                            }
                        })
                    }
                });
                
                dynamic included;
                if (include == "SubCategories") 
                {
                    included = categories
                        .SelectMany(x => x.SubCategories)
                        .Select(x => new
                        {
                            type = "category_leafs",
                            id = x.ID,
                            attributes = new
                            {
                                name = x.Name
                            }
                        });
                }
                else
                {
                    included = Enumerable.Empty<CategoryLeaf>();
                }

                return Ok(new
                {
                     links = new {
                         self = "/api/categories/"
                     },
                     data = data,
                     included = included
                });
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
