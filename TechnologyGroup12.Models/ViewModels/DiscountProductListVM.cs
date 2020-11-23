using System;
using System.Collections.Generic;
using System.Text;
using TechnologyGroup12.Models.Models;

namespace TechnologyGroup12.Models.ViewModels
{
    public class DiscountProductListVM
    {
        public string DiscountId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual Product Product { get; set; }
    }
}
