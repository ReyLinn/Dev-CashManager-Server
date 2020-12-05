using CashManager.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Controllers
{
    public class UserController : Controller
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public JsonResult Login(string username, string password)
        {
            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = _userService.GetUserByLogins(username, password);
                if(user != null)
                {
                    return Json(new { success = true, user = user });
                }
            }

            return Json(new { success = false });
        }

        public JsonResult Pay(int userId, float ammount)
        {
            if(ammount > 0)
            {
                var user = _userService.GetUserById(userId);
                if(user != null)
                {
                    _userService.Pay(userId, ammount)
                }
            }
        }
    }
}
