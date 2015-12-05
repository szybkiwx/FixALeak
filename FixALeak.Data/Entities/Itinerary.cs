using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.Data.Entities
{
    public class Itinerary : IEntity
    {
        public int ID { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
    }
}
