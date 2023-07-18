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
        //public DbSet <Category> Categories { get; set; }

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
        }
    }
}


