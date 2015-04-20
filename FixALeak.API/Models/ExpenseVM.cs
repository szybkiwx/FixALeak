using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FixALeak.API.Models
{
    public class ExpenseVM
    {
        public int ID { get; set; }
        public decimal Value { get; set; }
        public string Title { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual CategoryVM Category { get; set; }
    }
}