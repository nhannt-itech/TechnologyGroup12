using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnologyGroup12.Models.Models
{
    public partial class Account
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int Role { get; set; }
        [Required]
        public Guid? EmployeeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
