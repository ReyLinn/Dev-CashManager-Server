using System.ComponentModel.DataAnnotations;

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
