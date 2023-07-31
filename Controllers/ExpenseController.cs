using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.DataTransferObject;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinanceChecker.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpenseController(ILogger<ExpenseController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var userId = Guid.Parse(user.Id); // Convert userId from string to Guid

            // Calculate all user TotalTransactions for current week
            var currentWeekTotal = CalculateCurrentWeekTotal(userId);

            // Calculate  all user TotalTransactions for current month
            var currentMonthTotal = CalculateCurrentMonthTotal(userId);

            // Calculate all user TotalTransactions for previous week
            var previousWeekTotal = CalculatePreviousWeekTotal(userId);

            // Calculate  all user TotalTransactions for previous month
            var previousMonthTotal = CalculatePreviousMonthTotal(userId);

            // Calculate  for each Category of the user transactions
            var categoryMonthlySpent = CalculateCategoryMonthlySpent(userId);
            var categoryWeeklySpent = CalculateCategoryWeeklySpent(userId);
            var categoryPreviousWeekSpent = CalculateCategoryPreviousWeekSpent(userId);
            var categoryPreviousMonthSpent = CalculateCategoryPreviousMonthSpent(userId);

            // COMPARE PREVIOUS WEEKS
            var comparePreviousWeeks = ComparePreviousWeeks(userId);

            // COMPARE PREVIOUS MONTHS
            var comparePreviousMonths = ComparePreviousMonths(userId);

            var model = new ExpenseViewModel
            {
                CurrentWeekTotal = currentWeekTotal,
                CurrentMonthTotal = currentMonthTotal,
                PreviousWeekTotal = previousWeekTotal,
                PreviousMonthTotal = previousMonthTotal,
                CategoryMonthlySpent = categoryMonthlySpent,
                CategoryWeeklySpent = categoryWeeklySpent,
                CategoryPreviousWeekSpent = categoryPreviousWeekSpent,
                CategoryPreviousMonthSpent = categoryPreviousMonthSpent,
                ComparePreviousWeeks = comparePreviousWeeks,
                ComparePreviousMonths = comparePreviousMonths
            };

            return View(model);
        }


        // Helper methods for calculating totals and expenses
        private decimal CalculateCurrentWeekTotal(Guid userId)
        {
            var currentDate = DateTime.Now.Date;
            var startOfWeek = currentDate.AddDays(-((int)currentDate.DayOfWeek));
            var endOfWeek = startOfWeek.AddDays(6);

            // Calculate the total for the current week
            var total = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfWeek && t.Date <= endOfWeek)
                .Sum(t => t.Amount);

            return total;
        }

        private decimal CalculateCurrentMonthTotal(Guid userId)
        {
            var currentDate = DateTime.Today;
            var startOfMonth = currentDate.AddDays(1 - currentDate.Day);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Calculate the total for the current month
            var total = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfMonth && t.Date <= endOfMonth)
                .Sum(t => t.Amount);

            return total;
        }

        private decimal CalculatePreviousWeekTotal(Guid userId)
        {
            var currentDate = DateTime.Now.Date;
            var startOfPreviousWeek = currentDate.AddDays(-((int)currentDate.DayOfWeek) - 7);
            var endOfPreviousWeek = startOfPreviousWeek.AddDays(6);

            // Calculate the total for the previous week
            var total = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfPreviousWeek && t.Date <= endOfPreviousWeek)
                .Sum(t => t.Amount);

            return total;
        }

        private decimal CalculatePreviousMonthTotal(Guid userId)
        {
            var currentDate = DateTime.Now.Date;
            var startOfPreviousMonth = currentDate.AddDays(1 - currentDate.Day).AddMonths(-1);
            var endOfPreviousMonth = startOfPreviousMonth.AddMonths(1).AddDays(-1);

            // Calculate the total for the previous month
            var total = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfPreviousMonth && t.Date <= endOfPreviousMonth)
                .Sum(t => t.Amount);

            return total;
        }

        private Dictionary<string, decimal> CalculateCategoryMonthlySpent(Guid userId)
        {
            // Calculate the total monthly spent per category
            var monthlySpent = _db.Transactions
                .Where(t => t.UserID == userId)
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key!, g => g.Sum(t => t.Amount));

            return monthlySpent;
        }

        private Dictionary<string, decimal> CalculateCategoryWeeklySpent(Guid userId)
        {
            // Calculate the total weekly spent per category
            var currentDate = DateTime.Now.Date;
            var startOfWeek = currentDate.AddDays(-((int)currentDate.DayOfWeek));
            var endOfWeek = startOfWeek.AddDays(6);

            var weeklySpent = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfWeek && t.Date <= endOfWeek)
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key!, g => g.Sum(t => t.Amount));

            return weeklySpent;
        }

        private Dictionary<string, decimal> CalculateCategoryPreviousWeekSpent(Guid userId)
        {
            // Calculate the total spent per category for the previous week
            var currentDate = DateTime.Now.Date;
            var startOfPreviousWeek = currentDate.AddDays(-((int)currentDate.DayOfWeek) - 7);
            var endOfPreviousWeek = startOfPreviousWeek.AddDays(6);

            var previousWeekSpent = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfPreviousWeek && t.Date <= endOfPreviousWeek)
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key!, g => g.Sum(t => t.Amount));

            return previousWeekSpent;
        }

        private Dictionary<string, decimal> CalculateCategoryPreviousMonthSpent(Guid userId)
        {
            // Calculate the total spent per category for the previous month
            var currentDate = DateTime.Now.Date;
            var startOfPreviousMonth = currentDate.AddDays(1 - currentDate.Day).AddMonths(-1);
            var endOfPreviousMonth = startOfPreviousMonth.AddMonths(1).AddDays(-1);

            var previousMonthSpent = _db.Transactions
                .Where(t => t.UserID == userId && t.Date >= startOfPreviousMonth && t.Date <= endOfPreviousMonth)
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key!, g => g.Sum(t => t.Amount));

            return previousMonthSpent;
        }

        private Dictionary<string, decimal> ComparePreviousWeeks(Guid userId)
        {
            var comparePreviousWeeks = new Dictionary<string, decimal>();

            // Calculate and compare previous weeks' totals for the last 52 weeks (July 2022 to July 2023)
            for (int i = 1; i <= 12; i++)
            {
                var currentDate = DateTime.Now.Date;
                var startOfPreviousWeek = currentDate.AddDays(-((int)currentDate.DayOfWeek) - (7 * i));
                var endOfPreviousWeek = startOfPreviousWeek.AddDays(6);

                var previousWeekTotal = _db.Transactions
                    .Where(t => t.UserID == userId && t.Date >= startOfPreviousWeek && t.Date <= endOfPreviousWeek)
                    .Sum(t => t.Amount);

                // Add the week number (as a string) and its corresponding total to the dictionary
                comparePreviousWeeks.Add($"Week {i}", previousWeekTotal);
            }

            // Reverse the dictionary so that the most recent week comes first
            comparePreviousWeeks = comparePreviousWeeks.Reverse().ToDictionary(x => x.Key, x => x.Value);

            return comparePreviousWeeks;
        }

        private Dictionary<string, decimal> ComparePreviousMonths(Guid userId)
        {
            var comparePreviousMonths = new Dictionary<string, decimal>();

            // Calculate and compare previous months' totals for the last 12 months, including the current month
            for (int i = 0; i < 12; i++)
            {
                var currentDate = DateTime.Now.Date;
                var startOfPreviousMonth = currentDate.AddDays(1 - currentDate.Day).AddMonths(-i);
                var endOfPreviousMonth = startOfPreviousMonth.AddMonths(1).AddDays(-1);

                var previousMonthTotal = _db.Transactions
                    .Where(t => t.UserID == userId && t.Date >= startOfPreviousMonth && t.Date <= endOfPreviousMonth)
                    .Sum(t => t.Amount);

                // Add the month name (as a string) and its corresponding total to the dictionary
                comparePreviousMonths.Add(startOfPreviousMonth.ToString("MMMM yyyy", CultureInfo.InvariantCulture), previousMonthTotal);
            }

            // Reverse the dictionary so that the most recent month comes first
            comparePreviousMonths = comparePreviousMonths.Reverse().ToDictionary(x => x.Key, x => x.Value);

            return comparePreviousMonths;
        }


        private async Task<IdentityUser> GetUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}