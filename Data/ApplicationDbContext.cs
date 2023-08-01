using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FinanceChecker.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Humanizer;
using Microsoft.AspNetCore.Hosting;

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
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Savings> Savings { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<UserAlertSettings> UserAlertSettings { get; set; }
        public DbSet<ContactForm> ContactUs { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        //public DbSet<VideoTutorials> VideoTutorial { get; set; }
        public DbSet<Videos> AppVideo { get; set; }
        //public DbSet<Loans> Loan { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //          modelBuilder.Entity<nameofModelCLASS>().HasData(
            //);
            modelBuilder.Entity<Account>()
       .Ignore(a => a.BalanceForDay);

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

            modelBuilder.Entity<FAQ>().HasData(
       new FAQ
       {
           Id = 1,
           Question = "How do I sign up for the app?",
           Answer = "To sign up for our app, simply click on the \"Register\" button on the homepage, fill in the required information, and create a password to enable you login."
       },
       new FAQ
       {
           Id = 2,
           Question = "What do I do if I forget my password?",
           Answer = "If you forget your password, click on the \"Forgot Password\" link on the login page."
       },
        new FAQ
        {
            Id = 3,
            Question = "Can I view my total balance on the dashboard?",
            Answer = "Yes, the dashboard displays your total balance, including all your accounts, investments, and credit card balances."
        }, new FAQ
        {
            Id = 4,
            Question = "How can I track my expenses?",
            Answer = "Our app allows you to track expenses easily. First, categorise your transactions on the \"Transactions\" section,  then go to the \"Expense Tracking\" section,  and view detailed reports of your spending."
        },
         new FAQ
         {
             Id = 5,
             Question = "Can I link my bank accounts and credit cards to the app?",
             Answer = "Yes, you can link your bank accounts, credit cards, and investment accounts to the app and manually add details of your transactions and balances, we are working hard to ensure real-time tracking is available soon."
         },
          new FAQ
          {
              Id = 6,
              Question = "Is my financial data safe?",
              Answer = "We take the security of your data seriously. We use encryption techniques to protect your information, and you can enable multi-factor authentication for added security."
          },
           new FAQ
           {
               Id = 7,
               Question = "How do I set up budget goals?",
               Answer = "To set up budget goals, go to the \"Budgets\" section, and you can create and manage your budget goals based on different expense categories."
           },
            new FAQ
            {
                Id = 8,
                Question = "Can I receive alerts for specific transactions?",
                Answer = "Yes, you can set up personalised alerts for specific transactions, such as low balances, bills due date or unusual activities, to be notified immediately."
            },
             new FAQ
             {
                 Id = 9,
                 Question = "How do I update my profile information?",
                 Answer = "You can update your profile information in the \"User Profile\" section. Simply edit the relevant details, such as name, email, or contact information."
             },
               new FAQ
               {
                   Id = 10,
                   Question = "How do I get customer support?",
                   Answer = "We offer customer support through various channels. You can access our FAQ section, browse tutorials, or contact us via email for personalised assistance."
               }, new FAQ
               {
                   Id = 11,
                   Question = "How do I update my profile information?",
                   Answer = " No, we do not share your personal information with any third parties. Your data is strictly confidential and protected."
               }   );
        }

        public void SeedData()
        {
            if (!AppVideo.Any())
            {
                // If the video does not exist, seed it to the database
                var videoUrl = "https://vimeo.com/849958341";
                var video = new Videos
                {
                    Title = "Getting--",
                    VideoUrl = videoUrl
                };

                AppVideo.Add(video);
                SaveChanges();
            }
        }

        //     modelBuilder.Entity<VideoTutorial>().HasData(
        //new VideoTutorial
        //{
        //    Id = 1,
        //    Title = "Getting Started with the App",
        //    VideoFilePath = GetBase64Video("wwwroot/video/tutorialsample.mp4") // Update the file name accordingly
        //}
        //);

        //private string GetBase64Video(string videoFilePath)
        //{
        //    var sourceVideoFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, videoFilePath);
        //    byte[] videoBytes;
        //    using (var fileStream = new FileStream(sourceVideoFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            fileStream.CopyTo(memoryStream);
        //            videoBytes = memoryStream.ToArray();
        //        }
        //    }
        //    return Convert.ToBase64String(videoBytes);
        //}


    }
}


