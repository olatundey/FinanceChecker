using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceChecker.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]

    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetCurrentUserId();

            // Fetch the user's accounts
            var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();
            ViewBag.Id = userId;

            // Calculate total balance
            decimal totalBalance = accounts.Sum(a => a.Balance);
            ViewBag.TotalBalance = totalBalance;

            // Bank Account Balance for the current day
            decimal bankAccountBalanceCurrentDay = accounts
                .Where(a => a.AccountType == "BankAccount")
                .Sum(a => a.Balance);
            ViewBag.BankAccountBalanceCurrentDay = bankAccountBalanceCurrentDay;

            // Credit Card Balance for the current day
            decimal creditCardBalanceCurrentDay = accounts
                .Where(a => a.AccountType == "CreditCard")
                .Sum(a => a.Balance);
            ViewBag.CreditCardBalanceCurrentDay = creditCardBalanceCurrentDay;

            // Investment Account Balance for the current day
            decimal investmentBalanceCurrentDay = accounts
                .Where(a => a.AccountType == "Investment")
                .Sum(a => a.Balance);
            ViewBag.InvestmentBalanceCurrentDay = investmentBalanceCurrentDay;

            // Retrieve transactions for the last 5 days 
            DateTime currentDate = DateTime.Now;
            DateTime fiveDaysAgo = currentDate.AddDays(-5);
            var recentTransactions = _db.Transactions
                .Where(t => t.UserID == userId && t.CreatedAt >= fiveDaysAgo && t.CreatedAt <= currentDate)
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .ToList();
            ViewBag.RecentTransactions = recentTransactions;

            // Retrieve upcoming bills
            DateTime futureDate = currentDate.AddDays(30);
            var upcomingBills = _db.Bills
                .Where(b => b.UserID == userId && b.DueDate >= currentDate && b.DueDate <= futureDate)
                .OrderBy(b => b.DueDate)
                .Take(5)
                .ToList();
            ViewBag.UpcomingBills = upcomingBills;

            // Calculate total expenses per category for the current month
            var categoryMonthlySpent = CalculateCategoryMonthlySpent(userId);

            // an instance of the ExpenseViewModel and populate
            var model = new ExpenseViewModel
            {
                Accounts = accounts,
                CategoryMonthlySpent = categoryMonthlySpent
            };

            return View(model);
        }

        private Guid GetCurrentUserId()
        {
            return new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }

            // Calculate the total monthly spent per category
            private Dictionary<string, decimal> CalculateCategoryMonthlySpent(Guid userId)
        {
            var monthlySpent = _db.Transactions
                .Where(t => t.UserID == userId)
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

            return monthlySpent;
        }
    }
}
