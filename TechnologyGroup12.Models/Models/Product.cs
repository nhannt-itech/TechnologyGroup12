using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Product
    {
        public Product()
        {
            BillDetail = new HashSet<BillDetail>();
            DiscountProduct = new HashSet<DiscountProduct>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int? NumberInStock { get; set; }
        public int Price { get; set; }
        public bool? OnSale { get; set; }
        public long? ManufacturerId { get; set; }
        public long? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<BillDetail> BillDetail { get; set; }
        public virtual ICollection<DiscountProduct> DiscountProduct { get; set; }
    }
}
