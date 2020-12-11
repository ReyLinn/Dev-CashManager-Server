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
using System.Linq;

namespace CashManager.Services.Tests
{
    [TestClass()]
    public class UserServiceTest
    {

        /*
         * Test methode GetUserByLogin - SUCCESS // All good
         * @author Barthelmebs Alexis
         * @Summary Test la méthode GetUserByLogin, créé une base de donnée locale stockée en mémoire selon le schéma options. 
         * Ajoute un utilisateur, sauvegarde les modifications et exécute la méthode GetUserByLogins et compare le résultat.
         * 
         */
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

        /*
         * Test methode GetUserByLogin - FAIL // username et password en arguments de recherche ne correspondent pas aux entrées de la base de donnée.
         * @author Barthelmebs Alexis
         * @Summary Test le retour de la méthode GetUSerByLogins d'une requête avec de mauvaises informations, créé une base de donnée locale stockée en mémoire selon le schéma options. 
         * Ajoute un utilisateur, sauvegarde les modifications et exécute la méthode GetUserByLogins et vérifie si le résultat est nul.
         * 
         */


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

        /*
         * 
         * Test methode GetUserById - SUCCESS // all good
         * @author Barthelmebs Alexis
         * @Summary Test la méthode GetUserById qui à partir d'un Id retourne toutes les informtations d'un user. Créé une base de donnée locale stockée en mémoire selon le schéma options. 
         * Ajoute un utilisateur, sauvegarde les modifications et exécute la méthode GetUserByLogins et compare le résultat.
         * 
         */

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

            User userBase = new User
            {
                Id = 1,
                Username = "Username1",
                Password = "Password1",
                NbOfWrongCheques = 0,
                NbOfWrongCards = 0,
                BankAccount = null
            };

            Assert.AreEqual(user.Id, userBase.Id, "Should be equals");
            Assert.AreEqual(user.Username, userBase.Username, "Should be equals");
            Assert.AreEqual(user.Password, userBase.Password, "Should be equals");
            Assert.AreEqual(user.NbOfWrongCheques, userBase.NbOfWrongCheques, "Should be equals");
            Assert.AreEqual(user.NbOfWrongCards, userBase.NbOfWrongCards, "Should be equals");
            Assert.AreEqual(user.BankAccount, userBase.BankAccount, "Should be equals");
        }


        /*
         * 
         * Test methode GetUserById - FAIL // Arguement de recherche ne correspond à aucune entrée de base de donnée.
         * @author Barthelmebs Alexis
         * @Summary Test la méthode GetUserById qui à partir d'un Id retourne toutes les informtations d'un user. Créé une base de donnée locale stockée en mémoire selon le schéma options. 
         * Ajoute un utilisateur, sauvegarde les modifications et exécute la méthode GetUserByLogins et compare le résultat.
         * 
         */
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

        /*
         * 
         * Test methode Pay via CreditCard = true - SUCCESS // all good
         * @author Barthelmebs Alexis
         * @Summary Test la méthode Pay via carte de crédit avec un Id et un montant valide. Créer une base de donnée locale et y ajoute un utilisateur et un compte bancaire. 
         * Exécute la méthode Pay et compare les résultats.
         * 
         */


        [TestMethod()]
        public void PayTestCreditCardSucess()
        {
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

        /*
         * 
         * Test methode Pay via CreditCard = false - SUCCESS // all good
         * @author Barthelmebs Alexis
         * @Summary Test la méthode Pay via cheque avec un Id et un montant valide. Créer une base de donnée locale et y ajoute un utilisateur et un compte bancaire. 
         * Exécute la méthode Pay et compare les résultats.
         * 
         */

        [TestMethod]
        public void PayTestChequeSucess()
        {
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

        /*
         * 
         * Test methode Pay via CreditCard = false - FAIL // Nombre de cheques refusé excessif - 
         * @author Barthelmebs Alexis
         * @Summary Test la méthode Pay via cheque avec un Id et un montant valide. Créer une base de donnée locale et y ajoute un utilisateur ayant excédé le nombre de cheques refusé et un compte bancaire. 
         * Exécute la méthode Pay et compare les résultats.
         * 
         */

        [TestMethod]
        public void PayTestChequeFailAttempt()
        {
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

        /*
        * 
        * Test methode Pay via CreditCard = false - FAIL // fonds insuffisants 
        * @author Barthelmebs Alexis
        * @Summary Test la méthode Pay via cheque avec un Id et un montant valide. Créer une base de donnée locale. 
        * Ajoute un utilisateur et un compte bancaire dont le solde est inférieur au montant à payer. 
        * Exécute la méthode Pay et compare les résultats.
        * 
        */

        [TestMethod]
        public void PayTestChequeFailFund()
        {
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


        /*
        * 
        * Test methode Pay via CreditCard = true - FAIL // nombre de refus de carte excessif 
        * @author Barthelmebs Alexis
        * @Summary Test la méthode Pay via cheque avec un Id et un montant valide. Créer une base de donnée locale. 
        * Ajoute un utilisateur dont le nombre de  payement par carte refusé est au delà de la limite et un compte bancaire. 
        * Exécute la méthode Pay et compare les résultats.
        * 
        */

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

        /*
        * 
        * Test methode Pay via CreditCard = true - FAIL // fonds insuffisant 
        * @author Barthelmebs Alexis
        * @Summary Test la méthode Pay via cheque avec un Id et un montant valide. Créer une base de donnée locale. 
        * Ajoute un utilisateur et un compte bancaire dont le solde est inférieur au montant à payer. 
        * Exécute la méthode Pay et compare les résultats.
        * 
        */

        [TestMethod]
        public void PayTestCreditCardFailFund()
        {
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

        /*
        * 
        * Test methode CheckCreditCard - SUCCESS // all good
        * @author Barthelmebs Alexis
        * @Summary Test la méthode CheckCreditCard. Dans le cadre du projet, le retour est fixé à true. On vérifie que le retour de méthode est donc bien true.
        * 
        */

        [TestMethod()]
        public void PayTestFailTooManyTransactions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "PayTestFailTooManyTransactions")
              .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var transactions = new List<Transaction>()
                {
                    new Transaction
                    {
                        Id = 1,
                        CreatedDate = DateTime.Now.AddSeconds(-1)
                    },
                    new Transaction
                    {
                        Id = 2,
                        CreatedDate = DateTime.Now.AddSeconds(-2)
                    },
                    new Transaction
                    {
                        Id = 3,
                        CreatedDate = DateTime.Now.AddSeconds(-3)
                    }
                };

                context.AddRange(transactions);
                context.SaveChanges();

                context.Users.Add(new User
                {
                    Id = 1,
                    Transactions = transactions,
                    NbOfWrongCards = 0,
                    NbOfWrongCheques = 0
                });
                context.SaveChanges();
                var userService = new UserService(context);
                var result = userService.Pay(1, 100, true);
                Assert.AreEqual(result.Item2, "Maximum of transactions per minute reached.");
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

        /*
        * 
        * Test methode CheckCheque - SUCCESS // all good
        * @author Barthelmebs Alexis
        * @Summary Test la méthode CheckCheque. Dans le cadre du projet, le retour est fixé à true. On vérifie que le retour de méthode est donc bien true.
        * 
        *//

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