using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
