using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        public string Name { get; set; }
        [Required]
        [Remote("CheckAge", "Employee", ErrorMessage = "Employee must be over 18 years.")]
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [Remote("CheckEmail", "Employee", ErrorMessage = "Email is Invalid.")]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        public long? JobPositionId { get; set; }
        public int? YearEmployee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> JobPositionList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public virtual JobPosition JobPosition { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Dependents> Dependents { get; set; }
    }
}
