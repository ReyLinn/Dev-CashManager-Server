using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CashManager.Models;
using Moq;
using CashManager.Services;

namespace CashManager.Controllers.Tests
{

    //mock LOGIN -> username et password


    [TestClass()]
    public class UserControllerTest
    {
        /**
         * Test methode Login - SUCCESS // Good Login
         * @Author: Barthelmebs Alexis
         * @Summary: Test la méthode Login. Mock le retour de la méthode GetUSerByLogins. Compare les résultats qui doivent être égaux.
         */

        [TestMethod]
        public void LoginTestSucces()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.GetUserByLogins("Username1", "Password1"))
                .Returns(new User
                {
                    Id = 1,
                });

            var userController = new UserController(mock.Object, null);
            var jsonString = userController.Login("Username1", "Password1");

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = True, Id = 1 }";

            Assert.AreEqual(resultGen, resultBase , "Should be ok");
        }

        /**
         * Test methode Login - SUCCESS // Wrong Username
         * @author Barthelmebs Alexis
         * @Summary Test les retours d'erreur sur le username de la méthode Login. Mock le retour de la méthode GetUSerByLogins. Compare les résultats qui doivent être égaux.
         */
        [TestMethod]
        public void LoginTestFailUser()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.GetUserByLogins("Username1", "Password1"))
                .Returns(new User
                {
                    Id = 1,
                });

            var userController = new UserController(mock.Object, null);
            var jsonString = userController.Login("Wrong", "Password1");

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = False, message = User not found. }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
        }

        /**
         * Test methode Login - SUCCESS // Wrong Password
         * @author Barthelmebs Alexis
         * @Summary Test les retours d'erreur sur le password de la méthode Login. Mock le retour de la méthode GetUSerByLogins. Compare les résultats qui doivent être égaux.
         */
        [TestMethod]
        public void LoginTestFailPass()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.GetUserByLogins("Username1", "Password1"))
                .Returns(new User
                {
                    Id = 1,
                });

            var userController = new UserController(mock.Object, null);
            var jsonString = userController.Login("Username1", "Wrong");

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = False, message = User not found. }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
        }

        /**
         * Test methode GetPriceTest - SUCCESS // good product ref
         * @author Barthelmebs Alexis
         * @Summary Test la réussite de la méthode GetProductPriceTest. Mock le retour de la méthode GetProductByReference. Compare les résultats qui doivent être égaux.
         */
        [TestMethod]
        public void GetProductPriceTestSucces()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(x => x.GetProductByReference("00000001"))
                .Returns(new Product
                 {
                     Price = 10,
                 });

            var userController = new UserController(null, mock.Object);
            var jsonString = userController.GetProductPrice("00000001");

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = True, price = 10 }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
        }

        /**
         * Test methode GetPriceTest - SUCCESS // wrong product ref
         * @author Barthelmebs Alexis
         * @Summary Test le retour d'erreur de référence de la méthode GetProductPriceTest. Mock le retour de la méthode GetProductByReference. Compare les résultats qui doivent être égaux.
         */
        [TestMethod]
        public void GetProductPriceTestFail()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(x => x.GetProductByReference("00000002"))
                .Returns(new Product
                {
                    Price = 10,
                });

            var userController = new UserController(null, mock.Object);
            var jsonString = userController.GetProductPrice("00000001");

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = False, message = Product not found. }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
        }

        /**
         * Test methode Pay - SUCCESS // all good
         * @author Barthelmebs Alexis
         * @Summary Test la réussite de la méthode Pay. Mock le retour de la méthode Pay de l'interface IUserService et le retour de GetUserById. Compare les résultats qui doivent être égaux.
         */
        [TestMethod]
        public void PayTestSuccess()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.Pay(1, 100, true))
                .Returns(new Tuple<bool, string>(true, "Payement validated."));
            mock.Setup(x => x.GetUserById(1))
                .Returns(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });

            var userController = new UserController(mock.Object, null);
            var jsonString = userController.Pay(1, 100, true);

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = True, message = Payement validated. }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
        }



        /**
        * Test methode Pay - SUCCESS // fail getUserbyId
        * la méthode Pay de UserService est testée dans UserServiceTest
        * @author Barthelmebs Alexis
        * @Summary Test le retour d'erreur par mauvais utilisateur de la méthode Pay. Mock le retour de la méthode Pay de l'interface IUserService et le retour de GetUserById. Compare les résultats qui doivent être égaux.
        */
        [TestMethod]
        public void PayTestFailByGetUser()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.Pay(1, 100, true))
                .Returns(new Tuple<bool, string>(true, "Payement validated."));

            var userController = new UserController(mock.Object, null);
            var jsonString = userController.Pay(1, 100, true);

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = False, message = User not found. }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
        }
    }
}