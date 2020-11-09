using System;
using System.Collections.Generic;
using System.Text;

namespace TechnologyGroup12.Models.ViewModels
{
    public class ProductListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? NumberInStock { get; set; }
        public int Price { get; set; }
        public bool? OnSale { get; set; }
        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
    }
}
