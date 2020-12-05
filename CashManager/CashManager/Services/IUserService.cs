using CashManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Services
{
    interface IUserService
    {
        public User GetUserByLogins(string username, string password);

        public User GetUserById(int userId);

        public bool Pay(int userId, float ammount);
    }
}
