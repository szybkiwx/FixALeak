using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FixALeak.Data.Entities
{
    public class Expense : IEntity
    {
        public int ID { get; set; }
        public decimal Value { get; set; }
        public string Title { get; set; }
        public int CategoryLeafID { get; set; }
        [ForeignKey("CategoryLeafID")]
        public virtual CategoryLeaf Category { get; set; }
    }
}