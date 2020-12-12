using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnologyGroup12.Models.Models
{
    public partial class Discount
    {
        public Discount()
        {
            DiscountProduct = new HashSet<DiscountProduct>();
        }

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Remote("CheckLimitDiscount", "Discount", ErrorMessage = "Discount Value Must Be From 0 To 1 (Exam: 0.5 Means 50%)")]
        public double DiscountValue { get; set; }

        [Required]
        [Remote("CheckDateDiscount", "Discount", ErrorMessage = "StartDate Must Before EndDate", AdditionalFields = nameof(EndDate))]
        public DateTime StartDate { get; set; }

        [Required]
        [Remote("CheckDateDiscount", "Discount", ErrorMessage = "EndDate Must After StartDate", AdditionalFields = nameof(StartDate))]
        public DateTime EndDate { get; set; }

        public virtual ICollection<DiscountProduct> DiscountProduct { get; set; }
    }
}
