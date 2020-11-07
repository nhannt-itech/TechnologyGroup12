using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Product = new HashSet<Product>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Nation { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
