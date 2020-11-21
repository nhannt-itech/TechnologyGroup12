using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnologyGroup12.Models.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetail = new HashSet<BillDetail>();
        }

        public long Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public double TotalPriceBill { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> CustomerList { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<BillDetail> BillDetail { get; set; }
    }
}
