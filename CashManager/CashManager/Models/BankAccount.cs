using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CashManager.Models
{
    /// <summary>
    /// The User's BankAccount
    /// </summary>
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public float Balance { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public User Owner { get; set; }
    }
}
