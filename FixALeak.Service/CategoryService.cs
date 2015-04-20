using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;
using FixALeak.Data;

namespace FixALeak.Service
{
    public class CategoryService : ICategoryService
    {
        private ExpenseContext _ctx;

        public CategoryService(ExpenseContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Category> GetCategories(Guid userId)
        {
            return _ctx.Categories.Where(x => x.UserId == userId);
        }
    }
}
