using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnologyGroup12.Models.Models
{
    public partial class DiscountProduct
    {
        public string DiscountId { get; set; }
        [Required]
        public long ProductId { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual Product Product { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> ProductList { get; set; }
    }
}
