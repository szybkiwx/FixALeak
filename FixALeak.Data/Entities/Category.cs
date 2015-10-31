﻿using System;
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

    }
}