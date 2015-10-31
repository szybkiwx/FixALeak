using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;

namespace FixALeak.Service
{
    public interface ICategoryLeafService
    {
        CategoryLeaf Add(CategoryLeaf categoryLeaf);
        bool Exists(int cat, string name);
        CategoryLeaf Get(int id);
        CategoryLeaf Remove(int id);
        IEnumerable<CategoryLeaf> GetCategoryLeaves(int catId);
        IEnumerable<CategoryLeaf> GetCategoryLeavesWithIncludes(int id, string include);
        CategoryLeaf GetWithIncludes(int id, string include);
    }
}
