using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnologyGroup12.Models.Models
{
    public partial class JobPosition
    {
        public JobPosition()
        {
            Employee = new HashSet<Employee>();
        }

        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double BasicSalary { get; set; }
        [Required]
        public double Salary { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
