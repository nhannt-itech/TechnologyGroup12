﻿using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Category
    {
        public Category()
        {
            InverseCategoryNavigation = new HashSet<Category>();
            Product = new HashSet<Product>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? CategoryId { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual ICollection<Category> InverseCategoryNavigation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}