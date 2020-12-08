using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnologyGroup12.Models.Models
{
    public partial class Dependents
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Birth { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
