using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data;
using FixALeak.Data.Entities;
using System.Data.Entity.Infrastructure;

namespace FixALeak.Service
{
    public class CategoryLeafService : ICategoryLeafService
    {
        private IExpenseContext _ctx;

        public CategoryLeafService(IExpenseContext ctx)
        {
            _ctx = ctx;
        }
        public CategoryLeaf Add(CategoryLeaf categoryLeaf)
        {
            var result = _ctx.CategoryLeaves.Add(categoryLeaf);
            _ctx.SaveChanges();
            return result;
        }

        public bool Exists(int cat, string name)
        {
            return _ctx.CategoryLeaves.Where(x => x.CategoryID == cat).Any(x => x.Name == name);
        }

        public CategoryLeaf Get(int id)
        {
            return _ctx.CategoryLeaves.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<CategoryLeaf> GetCategoryLeaves(int catId)
        {
            return _ctx.CategoryLeaves.Where(x => x.CategoryID == catId);
        }

        public IEnumerable<CategoryLeaf> GetCategoryLeavesWithIncludes(int catId, string include)
        {
            DbQuery<CategoryLeaf> categoryLeavesSet = _ctx.CategoryLeaves;
            include.Split(new char[] { ',' }).ToList().ForEach(token =>
             {
                 var navProperty = typeof(CategoryLeaf).GetProperties().FirstOrDefault(prop => prop.Name.ToLower() == token);
                 if(navProperty != null)
                 {
                     categoryLeavesSet = categoryLeavesSet.Include(navProperty.Name);
                 }
                 else
                 {
                     throw new NavigationPropertyDoesNotExistException(String.Format("No property {} on object", token));
                 }
             });

            return categoryLeavesSet.Where(cl => cl.CategoryID == catId);
        }

        public CategoryLeaf GetWithIncludes(int id, string include)
        {
            throw new NotImplementedException();
        }

        public CategoryLeaf Remove(int id)
        {
            var toRemove = _ctx.CategoryLeaves.FirstOrDefault(x => x.ID == id);
            _ctx.CategoryLeaves.Remove(toRemove);
            return toRemove;
        }
    }
}
