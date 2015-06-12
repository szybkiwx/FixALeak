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
namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/categoryleaves")]
    //[Authorize]
    public class CategoryLeafController : ApiController
    {
        private ICategoryLeafService _categoryLeafService;

        public CategoryLeafController(ICategoryLeafService categoryLeafService)
        {
            _categoryLeafService = categoryLeafService;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = SerializerBuilder.Create().Serialize(new List<CategoryLeaf>() { new CategoryLeaf()
            {
                ID = 1,
                Name = "sdfsdfsdf",
                CategoryID = 4,
                Category = new Category() {
                    ID = 4,
                    Name = "cat 1",
                    
                },
                Expenses = new List<Expense>()
                {
                    new Expense() {
                        ID = 1,
                        Title = "sdfsdf"
                    },
                    new Expense() {
                        ID = 2,
                        Title = "ssssssssdf"
                    }
                }
            }});
            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Add(string json)
        {

         
            /**Mapper.CreateMap<CategoryLeafVM, CategoryLeaf>();
            Mapper.CreateMap<Category, CategoryVM>();
            var entity = Mapper.Map<CategoryLeafVM, CategoryLeaf>(model);
            entity.CategoryID = categoryId;

            var created = _categoryLeafService.Add(entity);
            return Ok(Mapper.Map<Category, CategoryVM>(created));*/
            return Ok();
        }
    }
}
