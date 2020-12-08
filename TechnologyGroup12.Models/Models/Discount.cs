using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnologyGroup12.Models.Models
{
    public partial class Discount
    {
        public Discount()
        {
            DiscountProduct = new HashSet<DiscountProduct>();
        }

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double DiscountValue { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public virtual ICollection<DiscountProduct> DiscountProduct { get; set; }
    }
}
