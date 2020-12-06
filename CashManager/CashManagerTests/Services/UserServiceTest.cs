using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashManager.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using CashManager.Data;
using CashManager.Models;
using CashManager.Controllers;

namespace CashManager.Services.Tests
{
    [TestClass()]
    public class UserServiceTest
    {
        [TestMethod()]
        public void GetUserByLoginsTest()
        {
            var mock = new Mock<ApplicationDbContext>();
            mock.Setup(x => x.("Username1", "Password1"))
                .Returns(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NnOfWrongCards = 0
                });

            var userController = new UserController(mock.Object, null);
            var jsonString = userController.Login("Username1", "Password1");

            var resultGen = (jsonString.Value).ToString();
            var resultBase = "{ success = True, Id = 1 }";

            Assert.AreEqual(resultGen, resultBase, "Should be ok");
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserByLoginsTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PayTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckCreditCardTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckChequeTest()
        {
            Assert.Fail();
        }
    }
}