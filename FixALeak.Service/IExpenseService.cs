using FixALeak.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.Service
{
    public interface IExpenseService
    {
        Expense Add(Expense expense);
        Expense Get(int id);
        IEnumerable<Expense> GetExpenses(int categoryLeafId);
        Expense Remove(int id);
        bool Exists(int id);
    }
}
