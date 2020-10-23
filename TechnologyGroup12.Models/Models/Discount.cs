using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Discount
    {
        public Discount()
        {
            DiscountProduct = new HashSet<DiscountProduct>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public double DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<DiscountProduct> DiscountProduct { get; set; }
    }
}
