using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnologyGroup12.Models.Models
{
    public partial class BillDetail
    {
        public Guid Id { get; set; }
        public long? BillId { get; set; }
        [Required]
        public long? ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> ProductList { get; set; }
    }
}
