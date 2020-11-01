﻿using System;
using System.Collections.Generic;

namespace TechnologyGroup12.Models.Models
{
    public partial class Dependents
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
