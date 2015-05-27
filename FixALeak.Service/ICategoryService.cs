﻿using System;
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
        //void DeleteCategory(int id);       
    }
}
