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
            var dbUser = new User
            {
                Id = 1,
                Username = "Username1",
                Password = "Password1",
            };

            mock.Object.Add(dbUser);
            //mock.Setup(x => x.Users.Add(new User {
            //    Id = 1,
            //    Username = "Username1",
            //    Password = "Password1",
            //}));

            var userService = new UserService(mock.Object);
            var user = userService.GetUserByLogins("Username1", "Password1");

            Assert.IsNotNull(user, "User should not be null");
            Assert.AreEqual(user.Id, 1, "User Id should be 1");
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