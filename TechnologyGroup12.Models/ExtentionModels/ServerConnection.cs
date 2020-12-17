using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechnologyGroup12.Models.ExtentionModels
{
    public class ServerConnection
    {
        public string connectionString { get; set; }
        [Required]
        public string serverName { get; set; }
        [Required]
        public string databaseName { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public IEnumerable<SelectListItem> databaseTable { get; set; }
    }
}
