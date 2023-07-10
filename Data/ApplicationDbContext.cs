using System;
using Microsoft.EntityFrameworkCore;

namespace FinanceChecker.Data
{
	public class ApplicationDbContext :DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //          modelBuilder.Entity<nameofModelCLASS>().HasData(
            //);

        }
    }
}

