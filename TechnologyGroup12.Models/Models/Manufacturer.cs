using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnologyGroup12.Models.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Product = new HashSet<Product>();
        }

        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Nation { get; set; }
        [Required]
        public int Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
