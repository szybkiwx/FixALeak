using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;

namespace FixALeak.Data
{
    public interface IExpenseContext
    {
        DbSet<Expense> Expenses { get; set; }
        DbSet<Category> Categories { get; set; }
        void SaveChanges();
    }
}
