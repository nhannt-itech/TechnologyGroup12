using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using TechnologyGroup12.Models.Models;

namespace TechnologyGroup12.Models.ViewModels
{
    public class EmployeeVM
    {
        public Employee Employee { get; set; }
        public IEnumerable<SelectListItem> JobPositionList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
    }
}
