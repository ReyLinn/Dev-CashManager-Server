using CashManager.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Controllers
{
    /// <summary>
    /// The User controller, main controller of the App
    /// </summary>
    public class UserController : Controller
    {
        //We define the services we need
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        /// <summary>
        /// The User Controller constructor
        /// </summary>
        /// <param name="userService">The UserService from the dependency injection</param>
        /// <param name="productService">The ProductService from the dependency injection</param>
        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        /// <summary>
        /// Default route
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpGet]
        public JsonResult Index()
        {
            return Json(new { message = "Hello There !" });
        }

        /// <summary>
        /// User Login Route
        /// </summary>
        /// <param name="username">User username</param>
        /// <param name="password">User password</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            //We check that username and password are not null or empty
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                //we get the user with the logins
                var user = _userService.GetUserByLogins(username, password);
                //if user is find
                if (user != null)
                {
                    //we return the user Id
                    return Json(new { success = true, user.Id });
                }
            }
            //if not we return success false
            return Json(new { success = false, message = "User not found." });
        }

        /// <summary>
        /// Get the product by its reference
        /// </summary>
        /// <param name="productReference">The product's reference</param>
        /// <returns>JsonResult</returns>
        [HttpGet]
        public JsonResult GetProductPrice(string productReference)
        {
            //we get the product via it's reference
            var product = _productService.GetProductByReference(productReference);
            //if we got a product
            if (product != null)
            {
                //we return it's price
                return Json(new { success = true, price = product.Price });
            }
            //else we return success false
            return Json(new { success = false, message = "Product not found." });
        }

        /// <summary>
        /// User payments' Route
        /// </summary>
        /// <param name="userId">Id of the User doing the Transaction</param>
        /// <param name="ammount">The ammount of the transaction</param>
        /// <param name="isCreditCard">Is the transaction by CreditCard or not (thent it's Cheque)</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult Pay(int userId, float ammount, bool isCreditCard)
        {
            //We create a default Tuple to return the defaults data
            var result = Tuple.Create<bool, string>(false, "The ammount should be over 0.");
            //if the ammount is superior to 0
            if (ammount > 0)
            {
                //we get the user by it's Id
                var user = _userService.GetUserById(userId);
                //if we find a User
                if (user != null)
                {
                    //We call the service Method that let's the User Pay
                    result = _userService.Pay(userId, ammount, isCreditCard);
                    //We return the success and the message that we got from Pay()
                    return Json(new { success = result.Item1, message = result.Item2 });
                }
                //else we return success false and User not found
                return Json(new { success = result.Item1, message = "User not found." });
            }
            //else we return success false and Ammount should be over 0
            return Json(new { success = result.Item1, message = result.Item2 });
        }
    }
}
