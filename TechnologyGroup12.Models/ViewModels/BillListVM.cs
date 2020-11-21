using System;
using System.Collections.Generic;
using System.Text;

namespace TechnologyGroup12.Models.ViewModels
{
    public class BillListVM
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public double TotalPriceBill { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
