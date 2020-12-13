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

namespace CashManagerTests.Services
{
    [TestClass()]
    public class ProductServiceTest
    {
        [TestMethod()]
        public void GetProductByReferenceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetProductByReferenceTest")
              .Options;

            int productId = 1;
            string productName = "TestProduct1";
            float productPrice = 10;
            string productReference = "00000001";

            Product product;

            using (var context = new ApplicationDbContext(options))
            {
                var productService = new ProductService(context);
                context.Products.Add(new Product
                {
                    Id = productId,
                    Name = productName,
                    Price = productPrice,
                    Reference = productReference
                });
                context.SaveChanges();
                product = productService.GetProductByReference(productReference);
            }

            Assert.AreEqual(product.Id, productId, "Product Id should be " + productId);
            Assert.AreEqual(product.Name, productName, "Product name should be " + productName);
            Assert.AreEqual(product.Price, productPrice, "Product Price should be " + productPrice);
            Assert.AreEqual(product.Reference, productReference, "Product Reference should be " + productReference);
        }

        [TestMethod()]
        public void GetProductByReferenceTestNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetProductByReferenceTestNull")
              .Options;

            string productReference = "00000001";

            Product product;

            using (var context = new ApplicationDbContext(options))
            {
                var productService = new ProductService(context);
                product = productService.GetProductByReference(productReference);
            }

            Assert.AreEqual(product, null, "Product Id should be NULL");
        }
    }
}
