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
    [RoutePrefix("api/Category")]
    [Authorize]
    public class CategoryController : ApiController
    {
        private CategoryService _categoryService;

        public CategoryController(CategoryService catService) 
        {
            _categoryService = catService;
        }

        [Route("")]
        public List<CategoryVM> GetCategories() 
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            var result = _categoryService.GetCategories(userId).ToList();
            Mapper.CreateMap<Category, CategoryVM>();
            return Mapper.Map<List<Category>, List<CategoryVM>>(result);
                
        }

    }
}
