using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Bill = new HashSet<Bill>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsVip { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Bill> Bill { get; set; }
    }
}
