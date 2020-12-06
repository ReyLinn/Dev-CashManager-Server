using CashManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Product> Products { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<User>()
                .HasOne(u => u.BankAccount)
                .WithOne(b => b.Owner)
                .HasForeignKey<BankAccount>(b => b.OwnerId);

            builder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User);

            builder.Entity<Product>()
                .HasIndex(p => p.Reference)
                .IsUnique();

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


            builder.Entity<BankAccount>().HasData(
                new BankAccount
                {
                    Id = 1,
                    Balance = 10000,
                    OwnerId = 1
                }
            );

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
