using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.Data.Entities;

namespace FixALeak.Service
{
    public interface IItineraryService
    {
        Itinerary Get(int id);
        Itinerary Add(Itinerary itinerary);
        void Remove(int id);
        bool Exists(int id);
        IEnumerable<Expense> GetExpenses(int itineraryId);
    }
}
