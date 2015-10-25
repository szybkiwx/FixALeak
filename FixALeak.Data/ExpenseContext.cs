using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FixALeak.Data.Entities;

namespace FixALeak.Data
{
    public class ExpenseContext : DbContext, IExpenseContext
    {
        public ExpenseContext() : base("DefaultContext")
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLeaf> CategoryLeaves { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}