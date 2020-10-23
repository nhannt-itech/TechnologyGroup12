using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class JobPosition
    {
        public JobPosition()
        {
            Employee = new HashSet<Employee>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public double BasicSalary { get; set; }
        public double Salary { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
