using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;
using FixALeak.Data;

namespace FixALeak.Service
{
    public class ItineraryService : IItineraryService
    {
        private IExpenseContext _ctx;

        public ItineraryService(IExpenseContext ctx)
        {
            _ctx = ctx;
        }

        public Itinerary Add(Itinerary itinerary)
        {
            _ctx.Itineraries.Add(itinerary);
            _ctx.SaveChanges();
            return itinerary;
        }

        public bool Exists(int id)
        {
            return _ctx.Itineraries.Any(x => x.ID == id);
        }

        public Itinerary Get(int id)
        {
            return _ctx.Itineraries.Find(id);
        }

        public IEnumerable<Expense> GetExpenses(int itineraryId)
        {
            return _ctx.Itineraries.Find(itineraryId).Expenses;
        }

        public void Remove(int id)
        {
            var tr = _ctx.Itineraries.Find(id);
            _ctx.Itineraries.Remove(tr);
            _ctx.SaveChanges();
        }
    }
}
