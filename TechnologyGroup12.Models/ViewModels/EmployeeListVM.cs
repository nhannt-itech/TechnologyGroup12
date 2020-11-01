using System;
using System.Collections.Generic;
using System.Text;

namespace TechnologyGroup12.Models.ViewModels
{
    public class EmployeeListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string JobName { get; set; }
    }
}
