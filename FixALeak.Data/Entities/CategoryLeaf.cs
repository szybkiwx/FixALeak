using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.Data.Entities
{
    public class CategoryLeaf
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
