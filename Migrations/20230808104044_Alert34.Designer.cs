﻿// <auto-generated />
using System;
using FinanceChecker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinanceChecker.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230808104044_Alert34")]
    partial class Alert34
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FinanceChecker.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("InstitutionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.Property<string>("syncType")
                        .HasColumnType("longtext");

                    b.HasKey("AccountID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FinanceChecker.Models.Bill", b =>
                {
                    b.Property<int>("BillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("BillName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.HasKey("BillID");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("FinanceChecker.Models.Budget", b =>
                {
                    b.Property<int>("BudgetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Progress")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("TotalTransactionsAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.HasKey("BudgetID");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("FinanceChecker.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("longtext");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Groceries"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Restaurants"
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "Shopping"
                        },
                        new
                        {
                            CategoryId = 4,
                            CategoryName = "Entertainment"
                        },
                        new
                        {
                            CategoryId = 5,
                            CategoryName = "Utilities"
                        },
                        new
                        {
                            CategoryId = 6,
                            CategoryName = "Transportation"
                        },
                        new
                        {
                            CategoryId = 7,
                            CategoryName = "Travel"
                        },
                        new
                        {
                            CategoryId = 8,
                            CategoryName = "Health"
                        },
                        new
                        {
                            CategoryId = 9,
                            CategoryName = "Education"
                        },
                        new
                        {
                            CategoryId = 10,
                            CategoryName = "Insurance"
                        },
                        new
                        {
                            CategoryId = 11,
                            CategoryName = "Rent/Mortgage"
                        },
                        new
                        {
                            CategoryId = 12,
                            CategoryName = "Utilities"
                        },
                        new
                        {
                            CategoryId = 13,
                            CategoryName = "Electronics"
                        },
                        new
                        {
                            CategoryId = 14,
                            CategoryName = "Gifts/Donations"
                        },
                        new
                        {
                            CategoryId = 15,
                            CategoryName = "Personal Care"
                        },
                        new
                        {
                            CategoryId = 16,
                            CategoryName = "Fitness/Sports"
                        },
                        new
                        {
                            CategoryId = 17,
                            CategoryName = "Home Improvement"
                        },
                        new
                        {
                            CategoryId = 18,
                            CategoryName = "Investments"
                        },
                        new
                        {
                            CategoryId = 19,
                            CategoryName = "Taxes"
                        },
                        new
                        {
                            CategoryId = 20,
                            CategoryName = "Miscellaneous"
                        },
                        new
                        {
                            CategoryId = 21,
                            CategoryName = "Salary"
                        },
                        new
                        {
                            CategoryId = 22,
                            CategoryName = "Side Hustle"
                        });
                });

            modelBuilder.Entity("FinanceChecker.Models.ContactForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ContactUs");
                });

            modelBuilder.Entity("FinanceChecker.Models.FAQ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Answer")
                        .HasColumnType("longtext");

                    b.Property<string>("Question")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("FAQs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Answer = "To sign up for our app, simply click on the \"Register\" button on the homepage, fill in the required information, and create a password to enable you login.",
                            Question = "How do I sign up for the app?"
                        },
                        new
                        {
                            Id = 2,
                            Answer = "If you forget your password, click on the \"Forgot Password\" link on the login page.",
                            Question = "What do I do if I forget my password?"
                        },
                        new
                        {
                            Id = 3,
                            Answer = "Yes, the dashboard displays your total balance, including all your accounts, investments, and credit card balances.",
                            Question = "Can I view my total balance on the dashboard?"
                        },
                        new
                        {
                            Id = 4,
                            Answer = "Our app allows you to track expenses easily. First, categorise your transactions on the \"Transactions\" section,  then go to the \"Expense Tracking\" section,  and view detailed reports of your spending.",
                            Question = "How can I track my expenses?"
                        },
                        new
                        {
                            Id = 5,
                            Answer = "Yes, you can link your bank accounts, credit cards, and investment accounts to the app and manually add details of your transactions and balances, we are working hard to ensure real-time tracking is available soon.",
                            Question = "Can I link my bank accounts and credit cards to the app?"
                        },
                        new
                        {
                            Id = 6,
                            Answer = "We take the security of your data seriously. We use encryption techniques to protect your information, and you can enable multi-factor authentication for added security.",
                            Question = "Is my financial data safe?"
                        },
                        new
                        {
                            Id = 7,
                            Answer = "To set up budget goals, go to the \"Budgets\" section, and you can create and manage your budget goals based on different expense categories.",
                            Question = "How do I set up budget goals?"
                        },
                        new
                        {
                            Id = 8,
                            Answer = "Yes, you can set up personalised alerts for specific transactions, such as low balances, bills due date or unusual activities, to be notified immediately.",
                            Question = "Can I receive alerts for specific transactions?"
                        },
                        new
                        {
                            Id = 9,
                            Answer = "You can update your profile information in the \"User Profile\" section. Simply edit the relevant details, such as name, email, or contact information.",
                            Question = "How do I update my profile information?"
                        },
                        new
                        {
                            Id = 10,
                            Answer = "We offer customer support through various channels. You can access our FAQ section, browse tutorials, or contact us via email for personalised assistance.",
                            Question = "How do I get customer support?"
                        },
                        new
                        {
                            Id = 11,
                            Answer = " No, we do not share your personal information with any third parties. Your data is strictly confidential and protected.",
                            Question = "How do I update my profile information?"
                        });
                });

            modelBuilder.Entity("FinanceChecker.Models.Savings", b =>
                {
                    b.Property<int>("SavingsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("CurrentSavings")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Goal")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Progress")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("SavingsName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.HasKey("SavingsID");

                    b.ToTable("Savings");
                });

            modelBuilder.Entity("FinanceChecker.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("InstitutionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsBalanceUpdate")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.HasKey("TransactionID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FinanceChecker.Models.UserAlertSettings", b =>
                {
                    b.Property<int>("AlertID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("DueDateReminderAlertEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("HighBalanceAlertEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("HighBalanceThreshold")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("IncomeDepositedAlertEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LowBalanceAlertEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("LowBalanceThreshold")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("OverspendingAlertEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SavingsGoalProgressAlertEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.HasKey("AlertID");

                    b.ToTable("UserAlertSettings");
                });

            modelBuilder.Entity("FinanceChecker.Models.Videos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AppVideo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FinanceChecker.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PostalCode")
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityAnswer")
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityQuestion")
                        .HasColumnType("longtext");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
