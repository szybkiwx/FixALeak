using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;

namespace FixALeak.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories(Guid userId);
        IEnumerable<Category> GetCategoryTree(Guid userId);
        Category AddCategory(Category cat);
        Category Get(int id);
        bool Exists(Guid userId, string name);
        Category Remove(int id);
        //void DeleteCategory(int id);       
    }
}
