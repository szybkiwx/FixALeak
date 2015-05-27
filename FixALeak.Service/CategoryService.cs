﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;
using FixALeak.Data;

namespace FixALeak.Service
{
    public class CategoryService : ICategoryService
    {
        private IExpenseContext _ctx;

        public CategoryService(IExpenseContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Category> GetCategories(Guid userId)
        {
            return _ctx.Categories.Where(x => x.UserId == userId);
        }


        public Category AddCategory(Category cat)
        {
            _ctx.Categories.Add(cat);
            _ctx.SaveChanges();
            return cat;
        }


        /*public void DeleteCategory(int id)
        {
            var rem = _ctx.Categories.FirstOrDefault(x => x.ID == id);
            _ctx.Categories.Remove(rem);
            _ctx.SaveChanges();
        }*/


        public IEnumerable<Category> GetCategoryTree(Guid userId)
        {
            return _ctx.Categories.Include(c => c.SubCategories).Where(x => x.UserId == userId);
        }
    }
}
