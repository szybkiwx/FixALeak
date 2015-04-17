using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FixALeak.API.Models
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext() : base("DefaultContext")
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}