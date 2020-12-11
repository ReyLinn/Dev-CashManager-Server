using System;
using System.ComponentModel.DataAnnotations;

namespace CashManager.Models
{
    /// <summary>
    /// The Transactions made by the Users
    /// </summary>
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public User User { get; set; }
    }
}
