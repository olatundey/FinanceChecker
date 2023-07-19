using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FinanceChecker.Models;

namespace FinanceChecker.Data
{
	public class ApplicationDbContext  : IdentityDbContext<IdentityUser>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet <Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //          modelBuilder.Entity<nameofModelCLASS>().HasData(
            //);

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(u => u.Accounts)
            //    .WithOne(a => a.ApplicationUser)
            //    .HasForeignKey(a => a.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(u => u.Alerts)
            //    .WithOne(al => al.ApplicationUser)
            //    .HasForeignKey(al => al.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //        modelBuilder.Entity<ApplicationUser>()
            //            .HasMany(u => u.Transactions)
            //.WithOne(t => t.ApplicationUser)
            //.HasForeignKey(t => t.Id)
            //.OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Account>()
            //.HasMany(a => a.Transactions)
            //.WithOne(t => t.Account)
            //.HasForeignKey(t => t.AccountID)
            //.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData(
        new Category { CategoryId = 1, CategoryName = "Groceries" },
        new Category { CategoryId = 2, CategoryName = "Restaurants" },
        new Category { CategoryId = 3, CategoryName = "Shopping" },
        new Category { CategoryId = 4, CategoryName = "Entertainment" },
        new Category { CategoryId = 5, CategoryName = "Utilities" },
        new Category { CategoryId = 6, CategoryName = "Transportation" },
        new Category { CategoryId = 7, CategoryName = "Travel" },
        new Category { CategoryId = 8, CategoryName = "Health" },
        new Category { CategoryId = 9, CategoryName = "Education" },
        new Category { CategoryId = 10, CategoryName = "Insurance" },
        new Category { CategoryId = 11, CategoryName = "Rent/Mortgage" },
        new Category { CategoryId = 12, CategoryName = "Utilities" },
        new Category { CategoryId = 13, CategoryName = "Electronics" },
        new Category { CategoryId = 14, CategoryName = "Gifts/Donations" },
        new Category { CategoryId = 15, CategoryName = "Personal Care" },
        new Category { CategoryId = 16, CategoryName = "Fitness/Sports" },
        new Category { CategoryId = 17, CategoryName = "Home Improvement" },
        new Category { CategoryId = 18, CategoryName = "Investments" },
        new Category { CategoryId = 19, CategoryName = "Taxes" },
        new Category { CategoryId = 20, CategoryName = "Miscellaneous" },
        new Category { CategoryId = 21, CategoryName = "Salary" },
        new Category { CategoryId = 22, CategoryName = "Side Hustle" });

        }

    }
}


