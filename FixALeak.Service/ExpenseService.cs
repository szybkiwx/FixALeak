using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;
using FixALeak.Data;

namespace FixALeak.Service
{
    public class ExpenseService : IExpenseService
    {
        private IExpenseContext _ctx;

        public ExpenseService(IExpenseContext ctx)
        {
            _ctx = ctx;
        }

        public Expense Add(Expense expense)
        {
           var added = _ctx.Expenses.Add(expense);
            _ctx.SaveChanges();
            return added;
        }

        public bool Exists(int id)
        {
            return _ctx.Expenses.Any(x => x.ID == id);
        }

        public Expense Get(int id)
        {
            return _ctx.Expenses.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Expense> GetExpenses(int categoryLeafId)
        {
            return _ctx.Expenses.Where(x => x.CategoryLeafID == categoryLeafId);
        }

        public Expense Remove(int id)
        {
            var rem = Get(id);
            _ctx.Expenses.Remove(rem);
            _ctx.SaveChanges();
            return rem;
        }
    }
}
