using CashManager.Data;
using CashManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CashManager.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext _context;
        
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserByLogins(string username, string password)
        {
            return _context.Users
                .Include(u => u.BankAccount)
                .Include(u => u.Transactions)
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User GetUserById(int userId)
        {
            return _context.Users
                .Include(u => u.BankAccount)
                .Include(u => u.Transactions)
                .FirstOrDefault(u => u.Id == userId);
        }

        public Tuple<bool, string> Pay(int userId, float ammount, bool isCreditCard)
        {
            var success = false;
            var message = "";
            var user = GetUserById(userId);
            if (user != null)
            {
                var jsonString = File.ReadAllText("PaymentConfig.json");
                var config = JsonSerializer.Deserialize<PaymentConfig>(jsonString);
                if(isCreditCard && CheckCreditCard() && user.NnOfWrongCards < config.NbOfWrongCards || !isCreditCard && CheckCheque() && user.NbOfWrongCheques < config.NbOfWrongCheques)
                {
                    if (user.Transactions.Where(t => t.CreatedDate <= DateTime.Now.AddMinutes(-1)).Count() < config.NumberOfTransactionPerMinute)
                    {
                        if (ammount <= config.MaximumCostOfTransaction && ammount <= user.BankAccount.Balance)
                        {
                            user.BankAccount.Balance -= ammount;
                            var transaction = new Transaction()
                            {
                                CreatedDate = DateTime.Now,
                                User = user
                            };
                            user.Transactions.Add(transaction);
                            _context.Transactions.Add(transaction);
                            _context.SaveChanges();
                            success = true;
                            message = "Payment validated.";
                        }
                    }
                    else
                    {
                        message = "Maximum of transactions per minutes reached.";
                    }
                }
                else if (isCreditCard)
                {
                    if(user.NnOfWrongCards < config.NbOfWrongCards)
                    {
                        message = "You have reached the maximum of failed cards.";
                    }
                    else
                    {
                        user.NnOfWrongCards++;
                        message = "Wrong credit card.";
                    }
                }
                else
                {
                    if (user.NnOfWrongCards < config.NbOfWrongCards)
                    {
                        message = "You have reached th maximum of failed cheques.";
                    }
                    else
                    {
                        user.NbOfWrongCheques++;
                        message = "Wrong cheque.";
                    }
                }
            }
            return new Tuple<bool, string>(success, message);
        }

        public bool CheckCreditCard()
        {
            return true;
        }

        public bool CheckCheque()
        {
            return true;
        }
    }
}
