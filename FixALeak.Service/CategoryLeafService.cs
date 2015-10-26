using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data;
using FixALeak.Data.Entities;
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

        public CategoryLeaf Remove(int id)
        {
            var toRemove = _ctx.CategoryLeaves.FirstOrDefault(x => x.ID == id);
            _ctx.CategoryLeaves.Remove(toRemove);
            return toRemove;
        }
    }
}
