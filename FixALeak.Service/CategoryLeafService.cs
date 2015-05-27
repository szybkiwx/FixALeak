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
        public CategoryLeaf Add(CategoryLeaf categoryLeaf, int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
