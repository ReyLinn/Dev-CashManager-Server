using CashManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        }
    }
}
