using CashManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Get a User by it's logins
        /// </summary>
        /// <param name="username">The User's username</param>
        /// <param name="password">The User's password</param>
        /// <returns>A User if logins match, if not returns null</returns>
        public User GetUserByLogins(string username, string password);

        /// <summary>
        /// Get a User by it's Id
        /// </summary>
        /// <param name="userId">The User Id</param>
        /// <returns>A User if the Id match, if not returns null</returns>
        public User GetUserById(int userId);

        /// <summary>
        /// Method for a User to Pay an Ammount by card or cheque 
        /// </summary>
        /// <param name="userId">The User Id</param>
        /// <param name="ammount">The Ammount to pay</param>
        /// <param name="isCreditCard">If we use a CreditCard or not</param>
        /// <returns>A Tuple<bool, string> which are the success and message returned</returns>
        public Tuple<bool, string> Pay(int userId, float ammount, bool isCreditCard);
    }
}
