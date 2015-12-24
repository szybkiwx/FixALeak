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
        DbSet<CategoryLeaf> CategoryLeaves { get; set; }
        DbSet<Itinerary> Itineraries { get; set; }
        void SaveChanges();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
