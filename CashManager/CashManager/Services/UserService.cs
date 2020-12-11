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
    /// <summary>
    /// The User Service
    /// </summary>
    public class UserService : IUserService
    {
        //We define a reference to our db context
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The USerService Constructor
        /// </summary>
        /// <param name="context">The DbContext from the dependency injection</param>
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a User by it's logins
        /// </summary>
        /// <param name="username">The User's username</param>
        /// <param name="password">The User's password</param>
        /// <returns>A User if logins match, if not returns null</returns>
        public User GetUserByLogins(string username, string password)
        {
            //We get the table Users from our DbContext
            return _context.Users
                //We include the BankAccount linked to our User
                .Include(u => u.BankAccount)
                //We include the Transactions linked to our User
                .Include(u => u.Transactions)
                //We get the first one to match the username and the password, or we return null
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        /// <summary>
        /// Get a User by it's Id
        /// </summary>
        /// <param name="userId">The User Id</param>
        /// <returns>A User if the Id match, if not returns null</returns>
        public User GetUserById(int userId)
        {
            //We get the table Users from our DbContext
            return _context.Users
                //We include the BankAccount linked to our User
                .Include(u => u.BankAccount)
                //We include the Transactions linked to our User
                .Include(u => u.Transactions)
                //We get the first one to match the Id, or we return null
                .FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>
        /// Method for a User to Pay an Ammount by card or cheque 
        /// </summary>
        /// <param name="userId">The User Id</param>
        /// <param name="ammount">The Ammount to pay</param>
        /// <param name="isCreditCard">If we use a CreditCard or not</param>
        /// <returns>A Tuple<bool, string> which are the success and message returned</returns>
        public Tuple<bool, string> Pay(int userId, float ammount, bool isCreditCard)
        {
            var dateTimeToCompare = DateTime.Now;
            //We initialize the returned variables of the method
            var success = false;
            var message = "";
            //We get the User by it's Id
            var user = GetUserById(userId);
            //If we find a User
            if (user != null)
            {
                //We read the Payment Configuration json file
                var jsonString = File.ReadAllText("PaymentConfig.json");
                //We deserialize the string to an PaymentConfig Object
                var config = JsonSerializer.Deserialize<PaymentConfig>(jsonString);

                //If the User pay by CreditCard AND the CreditCard is verified, AND the User didn't reached the maximum of wrong CreditCard
                if (isCreditCard
                    && CheckCreditCard()
                    && user.NbOfWrongCards < config.NbOfWrongCards
                //OR the User pay by Cheque AND the Cheque is verified, AND the User didn't reached the maximum of wrong Cheque
                || !isCreditCard
                    && CheckCheque()
                    && user.NbOfWrongCheques < config.NbOfWrongCheques)
                {
                    //If we found a Number of transactions made under a minute inferior the the maximum of transactions per minute
                    if (user.Transactions.Where(t => t.CreatedDate <= dateTimeToCompare && t.CreatedDate >= dateTimeToCompare.AddMinutes(-1)).Count() < config.NumberOfTransactionPerMinute)
                    {
                        //If the ammount the User wants to Pay is inferior to the maximum cost of transaction and the ammount is inferior to the User's BancAccount balance
                        if (ammount <= config.MaximumCostOfTransaction && ammount <= user.BankAccount.Balance)
                        {
                            //We retrieve the ammount to the User's BankAccount Balance
                            user.BankAccount.Balance -= ammount;
                            //We create a new Transaction
                            var transaction = new Transaction()
                            {
                                CreatedDate = DateTime.Now,
                                User = user
                            };
                            //We add the transaction to the User
                            user.Transactions.Add(transaction);
                            //We add the transaction to the Db
                            _context.Transactions.Add(transaction);
                            //We apply the changes to the Db
                            _context.SaveChanges();
                            //We set the return variables
                            success = true;
                            message = "Payment validated.";
                        }
                        //If the ammount is superior
                        else if (ammount > user.BankAccount.Balance)
                        {
                            message = "Your Bank Account's Balance is inferior to the ammount of your bill.";
                        }
                        //If the ammount is superior to the configuration maximum cost of transaction
                        else
                        {
                            message = $"The ammount has to be inferior to {config.MaximumCostOfTransaction}.";
                        }
                    }
                    //If the User reached the maximum transactions per minute
                    else
                    {
                        message = "Maximum of transactions per minute reached.";
                    }
                }
                //If something went wrong and the payment is by CreditCard
                else if (isCreditCard)
                {
                    //If the user has reached the maximum of failed cards.
                    if (user.NbOfWrongCards >= config.NbOfWrongCards)
                    {
                        message = "You have reached the maximum of failed cards.";
                    }
                    //else if the CreditCard was wrong
                    else
                    {
                        //We add 1 to the User's number of wrong cards
                        user.NbOfWrongCards++;
                        message = "Wrong credit card.";
                    }
                }
                //If something went wrong and the payment is by Cheque
                else
                {
                    //If the user has reached the maximum of failed cheque.
                    if (user.NbOfWrongCheques >= config.NbOfWrongCheques)
                    {
                        message = "You have reached th maximum of failed cheques.";
                    }
                    else
                    {
                        //We add 1 to the User's number of wrong cheques
                        user.NbOfWrongCheques++;
                        message = "Wrong cheque.";
                    }
                }
            }
            //We return the variables
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
