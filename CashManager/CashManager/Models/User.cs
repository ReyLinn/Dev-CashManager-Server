using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CashManager.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int NnOfWrongCards { get; set; }

        [Required]
        public int NbOfWrongCheques { get; set; }

        public BankAccount BankAccount { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
