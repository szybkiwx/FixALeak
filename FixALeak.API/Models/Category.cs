﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixALeak.API.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}