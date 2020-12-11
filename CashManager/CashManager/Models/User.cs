using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashManager.Models
{
    /// <summary>
    /// The Users of the application
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int NbOfWrongCards { get; set; }

        [Required]
        public int NbOfWrongCheques { get; set; }

        /// <summary>
        /// The BankAccount of the User
        /// </summary>
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// All the Transactions of the User
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; }
    }
}
