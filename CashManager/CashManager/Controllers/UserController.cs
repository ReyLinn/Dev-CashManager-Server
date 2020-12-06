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
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        [HttpGet]
        public JsonResult Index()
        {
            return Json(new { message  = "Hello There !" });
        }

        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = _userService.GetUserByLogins(username, password);
                if(user != null)
                {
                    return Json(new { success = true, user.Id });
                }
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult GetProductPrice(string productReference)
        {
            var product = _productService.GetProductByReference(productReference);
            if(product != null)
            {
                return Json(new { success = true, price = product.Price });
            }
            return Json(new { success = false, message = "Product not found." });
        }

        [HttpPost]
        public JsonResult Pay(int userId, float ammount, bool isCreditCard)
        {
            var result = Tuple.Create<bool, string>(false, "The ammount should be over 0.");
            if(ammount > 0)
            {
                var user = _userService.GetUserById(userId);
                if(user != null)
                {
                    result = _userService.Pay(userId, ammount, isCreditCard);
                }
            }
            return Json(new { success = result.Item1, message = result.Item2});
        }
    }
}
