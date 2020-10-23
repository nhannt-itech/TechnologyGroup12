using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Account = new HashSet<Account>();
            Bill = new HashSet<Bill>();
            Dependents = new HashSet<Dependents>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? JobPositionId { get; set; }
        public int? YearEmployee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual JobPosition JobPosition { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Dependents> Dependents { get; set; }
    }
}
