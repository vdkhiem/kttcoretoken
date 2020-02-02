using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Models
{
    public class Product :  EntityBase
    {
        [Required]
        public string Code { get; set; }
        public string Description { get; set; }
        
    }
}
