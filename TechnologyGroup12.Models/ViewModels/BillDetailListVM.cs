using System;
using System.Collections.Generic;
using System.Text;

namespace TechnologyGroup12.Models.ViewModels
{
    public class BillDetailListVM
    {
        public Guid Id { get; set; }
        public long? BillId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
