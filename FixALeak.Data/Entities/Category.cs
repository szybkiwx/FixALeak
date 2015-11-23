using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixALeak.Data.Entities
{
    public class Category : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public virtual ICollection<CategoryLeaf> SubCategories { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Category)obj;
            return other.ID == ID &&
                other.Name == Name &&
                other.UserId == UserId &&
                (other.SubCategories == null && SubCategories == null) || 
                (other.SubCategories != null && other.SubCategories.SequenceEqual(SubCategories));
        }

    }
}