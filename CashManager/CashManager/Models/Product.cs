using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Models
{
    /// <summary>
    /// The Products to pay for
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; set; } 
        
        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Reference { get; set; }
    }
}
