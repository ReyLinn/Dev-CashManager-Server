using CashManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        //We define the table Users 
        public DbSet<User> Users { get; set; }
        //We define the table Transactions 
        public DbSet<Transaction> Transactions { get; set; }
        //We define the table BankAccounts 
        public DbSet<BankAccount> BankAccounts { get; set; }
        //We define the table Products 
        public DbSet<Product> Products { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //We set a unique index on the User's Username
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            //We set a One to One relation between a User and BankAccount
            builder.Entity<User>()
                .HasOne(u => u.BankAccount)
                .WithOne(b => b.Owner)
                .HasForeignKey<BankAccount>(b => b.OwnerId);

            //We set a Many to One relation between a User and Transactions
            builder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User);

            //We set a unique index on the Product's Reference
            builder.Entity<Product>()
                .HasIndex(p => p.Reference)
                .IsUnique();

            //We seed data to the User's table
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1",
                    NbOfWrongCheques = 0,
                    NnOfWrongCards = 0
                }
            );

            //We seed data to the User's table
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Username = "Username2",
                    Password = "Password2",
                    NbOfWrongCheques = 2,
                    NnOfWrongCards = 2
                }
            );

            //We seed data to the BankAccount's table
            builder.Entity<BankAccount>().HasData(
                new BankAccount
                {
                    Id = 1,
                    Balance = 10000,
                    OwnerId = 1
                }
            );

            //We seed data to the Product's table
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Produit de test",
                    Price = 10,
                    Reference = "00000001"
                }
            );
        }
    }
}
