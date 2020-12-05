using CashManager.Data;
using CashManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool Pay(int userId, float ammount)
        {
            var user = GetUserById(userId);
            if(user != null)
            {

            }
        }
    }
}
