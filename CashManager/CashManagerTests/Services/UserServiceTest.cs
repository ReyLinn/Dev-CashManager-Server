using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashManager.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using CashManager.Data;
using CashManager.Models;
using CashManager.Controllers;
using Microsoft.EntityFrameworkCore;

namespace CashManager.Services.Tests
{
    [TestClass()]
    public class UserServiceTest
    {
        [TestMethod()]
        public void GetUserByLoginsTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetUserByLoginsTest")
              .Options;

            User user;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                user = userService.GetUserByLogins("Username1", "Password1");
            }


            Assert.AreEqual(user.Id, 1, "User Id should be 1");
        }

        [TestMethod()]
        public void GetUserByLoginsTestFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "GetUserByLoginsTestFail")
               .Options;

            User user;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username2",
                    Password = "Password2",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                user = userService.GetUserByLogins("Username1", "Password1");
            }

            Assert.IsNull(user);
        }


        [TestMethod()]
        public void GetUserByIdTestSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetUserByIdTestSuccess")
              .Options;

            User user;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                user = userService.GetUserById(1);
            }

            User userBase = new User();
            userBase.Id = 1;
            userBase.Username = "Username1";
            userBase.Password = "Password1";
            userBase.NbOfWrongCheques = 0;
            userBase.NbOfWrongCards = 0;
            userBase.BankAccount = null;
;


            Assert.AreEqual(user.Id, userBase.Id, "Should be equals");
            Assert.AreEqual(user.Username, userBase.Username, "Should be equals");
            Assert.AreEqual(user.Password, userBase.Password, "Should be equals") ;
            Assert.AreEqual(user.NbOfWrongCheques, userBase.NbOfWrongCheques, "Should be equals");
            Assert.AreEqual(user.NbOfWrongCards, userBase.NbOfWrongCards, "Should be equals");
            Assert.AreEqual(user.BankAccount, userBase.BankAccount, "Should be equals");
        }

        [TestMethod]
        public void GetUserByIdTestFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "GetUserByIdTestFail")
               .Options;

            User user;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                user = userService.GetUserById(2);
            }

            Assert.IsNull(user);

        }

        [TestMethod()]
        public void PayTestCreditCardSucess()
        {

            // GetUserById


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestCreditCardSucess")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                context.BankAccounts.Add(new BankAccount
                {
                    Id = 1,
                    Balance = 10000,
                    OwnerId = 1
                });
                context.SaveChanges();
                var resultGen = userService.Pay(1, 100, true);
                Assert.AreEqual(resultGen.Item1, true);
                Assert.AreEqual(resultGen.Item2, "Payment validated.");
            }


        }

        [TestMethod]
        public void PayTestChequeSucess()
        {

            // GetUserById


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestChequeSucess")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                context.BankAccounts.Add(new BankAccount
                {
                    Id = 1,
                    Balance = 10000,
                    OwnerId = 1
                });
                context.SaveChanges();
                var resultGen = userService.Pay(1, 100, false);
                Assert.AreEqual(resultGen.Item1, true);
                Assert.AreEqual(resultGen.Item2, "Payment validated.");
            }


        }

        [TestMethod]
        public void PayTestChequeFailAttempt()
        {

            // GetUserById


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestChequeFailAttempt")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 5,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                context.BankAccounts.Add(new BankAccount
                {
                    Id = 1,
                    Balance = 10000,
                    OwnerId = 1
                });
                context.SaveChanges();
                var resultGen = userService.Pay(1, 100, false);
                Assert.AreEqual(resultGen.Item1, false);
                Assert.AreEqual(resultGen.Item2, "You have reached th maximum of failed cheques.");
            }


        }
        [TestMethod]
        public void PayTestChequeFailFund()
        {

            // GetUserById


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestChequeFailFund")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                context.BankAccounts.Add(new BankAccount
                {
                    Id = 1,
                    Balance = 0,
                    OwnerId = 1
                });
                context.SaveChanges();
                var resultGen = userService.Pay(1, 100, false);
                Assert.AreEqual(resultGen.Item1, false);
                Assert.AreEqual(resultGen.Item2, "Your Bank Account's Balance is inferior to the ammount of your bill.");
            }


        }

        [TestMethod]
        public void PayTestCreditCardFailAttempt()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestCreditCardFailAttempt")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 5
                });
                context.SaveChanges();
                context.BankAccounts.Add(new BankAccount
                {
                    Id = 1,
                    Balance = 10000,
                    OwnerId = 1
                });
                context.SaveChanges();
                var resultGen = userService.Pay(1, 100, true);
                Assert.AreEqual(resultGen.Item1, false);
                Assert.AreEqual(resultGen.Item2, "You have reached the maximum of failed cards.");
            }


        }
        [TestMethod]
        public void PayTestCreditCardFailFund()
        {

            // GetUserById


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestCreditCardFailFund")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userService = new UserService(context);
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NbOfWrongCards = 0
                });
                context.SaveChanges();
                context.BankAccounts.Add(new BankAccount
                {
                    Id = 1,
                    Balance = 0,
                    OwnerId = 1
                });
                context.SaveChanges();
                var resultGen = userService.Pay(1, 100, true);
                Assert.AreEqual(resultGen.Item1, false);
                Assert.AreEqual(resultGen.Item2, "Your Bank Account's Balance is inferior to the ammount of your bill.");
            }


        }

        [TestMethod()]
        public void CheckCreditCardTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "CheckCreditCardTest")
               .Options;

                var context = new ApplicationDbContext(options);
                var userService = new UserService(context);

            Assert.IsTrue(userService.CheckCreditCard());

        }

        [TestMethod()]
        public void CheckChequeTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "CheckChequeTest")
               .Options;

            var context = new ApplicationDbContext(options);
            var userService = new UserService(context);

            Assert.IsTrue(userService.CheckCheque());
        }
    }
}