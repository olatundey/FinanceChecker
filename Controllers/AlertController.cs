using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.DataTransferObject;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    [Authorize]
    public class AlertController : Controller
    {
        private readonly ILogger<AlertController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AlertController(ILogger<AlertController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var alertSettings = await GetUserAlertSettings();

            decimal totalBalance = await GetTotalAccountBalance();

            var notifications = new List<dynamic>();

            if (alertSettings.HighBalanceAlertEnabled && totalBalance > alertSettings.HighBalanceThreshold)
            {
                notifications.Add(new { Message = "Your account balance is higher than the specified threshold. \n", Type = "info" });
                //return RedirectToAction("Notification");

            }

            if (alertSettings.LowBalanceAlertEnabled && totalBalance < alertSettings.LowBalanceThreshold)
            {
                notifications.Add(new { Message = "Your account balance is low. Please consider adding funds.\n", Type = "warning" });

            }
            var today = DateTime.Today;
            var incomeTransactions = GetIncomeTransactions();
            if (alertSettings.IncomeDepositedAlertEnabled && incomeTransactions.Any(t => t.Category == "Salary" || t.Category == "Paycheck"))
            {
                // To Check if there are any income transactions before finding most recent date
                if (incomeTransactions.Any())
                {
                    var mostRecentTransactionDate = incomeTransactions.Max(t => t.UpdatedAt);

                    if (mostRecentTransactionDate.Date >= today)
                    {
                        notifications.Add(new { Message = "Your salary or income has been deposited.\n", Type = "info" });
                    }
                }
            }

            if (alertSettings.DueDateReminderAlertEnabled)
            {
                var dueBills = GetDueBills();
                if (dueBills.Any())
                {
                    notifications.Add(new { Message = "You have bills due today.", Type = "info" });
                }
            }


            var targetReached = CheckTargetAmountReached();
            if (alertSettings.TargetAmountReachedAlertEnabled && targetReached)
            {
                notifications.Add(new { Message = "Your savings goal has been reached.", Type = "info" });
            }

            var isOverspending = CheckOverspending();
            if (alertSettings.OverspendingAlertEnabled && isOverspending)
            {
                notifications.Add(new { Message = "You have exceeded your budget in one or more categories.", Type = "warning" });
            }

            if (notifications.Count > 0)
            {
                // Concatenate multiple messages 
                var message = string.Join("<br><br>", notifications.Select(n => n.Message));
                TempData["AlertMessage"] = message;
                TempData["AlertType"] = "info"; // Use "success", "info", "warning", or "danger" for AlertType
                return RedirectToAction("Notification");
            }

            // Return the "Index" view with the alert settings
            return View(alertSettings);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AlertSettingsViewModel alertSettings)
        {
            await SaveUserAlertSettings(alertSettings);

            TempData["success"] = "Alert settings updated successfully.";
            return RedirectToAction("Index");
        }

        private bool CheckTargetAmountReached()
        {
            var userId = GetCurrentUserId();

            var savingsGoals = _db.Savings.Where(s => s.UserID == userId).ToList();

            foreach (var savingsGoal in savingsGoals)
            {
                if (savingsGoal.Goal > 0)
                {
                    var progress = (savingsGoal.CurrentSavings / savingsGoal.Goal) * 100;

                    if (progress >= 100)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private bool CheckOverspending()
        {
            var userId = GetCurrentUserId();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var budgets = _db.Budgets.Where(b => b.UserID == userId).ToList();

            foreach (var budget in budgets)
            {
                //var userId = GetCurrentUserId().ToString(); // Convert Guid to string

                var transactionsForCategory = _db.Transactions
                    .Where(t => t.Category == budget.CategoryName && t.UserID == userId &&
                                t.Date.Month == currentMonth && t.Date.Year == currentYear);

                var totalTransactionsAmount = transactionsForCategory.Sum(t => Math.Abs(t.Amount));
              
                if (Math.Abs(budget.Amount) > 0)
                {
                    var progress = (totalTransactionsAmount / Math.Abs(budget.Amount)) * 100;

                    if (progress > 100)
                    {
                        return true;
                    }

                }
            }

            return false;
        }



        public async Task<IActionResult> HighBalanceAlert()
        {
            _logger.LogInformation("Tunde HighBalanceAlert action is being executed.");

            var userSettings = await GetUserAlertSettings();

            decimal totalBalance = await GetTotalAccountBalance();

            if (userSettings.HighBalanceAlertEnabled && totalBalance > userSettings.HighBalanceThreshold)
            {
                TempData["AlertMessage"] = "Your account balance is higher than the specified threshold. You can verify and think about Investing";
                TempData["AlertType"] = "info";

                return RedirectToAction("Notification");
            }


            return View("Index", userSettings);
            //return null;
        }


        public async Task<IActionResult> LowBalanceAlert()
        {

            var userSettings = await GetUserAlertSettings();

            decimal totalBalance = await GetTotalAccountBalance();

            if (userSettings.LowBalanceAlertEnabled && totalBalance < userSettings.LowBalanceThreshold)
            {
                TempData["AlertMessage"] = "Your account balance is low. Please consider adding funds.";
                TempData["AlertType"] = "warning";

                return RedirectToAction("Notification");
            }

            return View("Index", userSettings);
        }

        public IActionResult Notification()
        {

            var alertMessage = TempData["AlertMessage"] as string;
            var alertType = TempData["AlertType"] as string;

            if (!string.IsNullOrEmpty(alertMessage) && !string.IsNullOrEmpty(alertType))
            {
                ViewBag.AlertMessage = alertMessage;
                ViewBag.AlertType = alertType;
            }

            var alertSettings = GetUserAlertSettings().GetAwaiter().GetResult();

            return View(alertSettings);
        }


        public async Task<IActionResult> IncomeAlerts()
        {
            var userSettings = await GetUserAlertSettings();
            var incomeTransactions = GetIncomeTransactions();


            if (userSettings.IncomeDepositedAlertEnabled && incomeTransactions.Any(t => t.Category == "Salary" || t.Category == "Paycheck"))
            {
                TempData["AlertMessage"] = "Your salary or income has been deposited.";
                TempData["AlertType"] = "info";

                return RedirectToAction("Notification");
            }

            return View("AlertSettings", GetUserAlertSettings());
        }



        private List<Transaction> GetIncomeTransactions()
        {
            var user = _userManager.GetUserAsync(User).Result;
            // var userId = user.Id;
            var userId = Guid.Parse(user.Id);


            var today = DateTime.Today;

            var incomeTransactions = _db.Transactions
                .Where(t => t.UserID == userId && (t.Category == "Salary" || t.Category == "Paycheck") && t.UpdatedAt.Date >= today)
                .ToList();

            return incomeTransactions;
        }



        private async Task<decimal> GetTotalAccountBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            //var userId = user.Id;
            var userId = Guid.Parse(user.Id);

            var accounts = _db.Accounts.Where(account => account.UserID == userId).ToList();
            decimal totalBalance = accounts.Sum(account => account.Balance);

            return totalBalance;
        }

        private List<Bill> GetDueBills()
        {
            //var user = _userManager.GetUserAsync(User).Result;
            //var userId = user.Id;
            var userId = GetCurrentUserId();
            var today = DateTime.Today;

            var dueBills = _db.Bills
                .Where(b => b.UserID == userId && b.DueDate.Date == today.Date)
                .ToList();

            return dueBills;
        }

        private async Task<AlertSettingsViewModel> GetUserAlertSettings()
        {
            var userId = GetCurrentUserId();

            var userSettings = _db.UserAlertSettings.FirstOrDefault(u => u.UserID == userId);

            if (userSettings == null)
            {
                userSettings = new UserAlertSettings
                {
                    UserID = userId
                };
                _db.UserAlertSettings.Add(userSettings);
                await _db.SaveChangesAsync();
            }

            var alertSettings = new AlertSettingsViewModel
            {
                LowBalanceAlertEnabled = userSettings.LowBalanceAlertEnabled,
                LowBalanceThreshold = userSettings.LowBalanceThreshold,
                HighBalanceAlertEnabled = userSettings.HighBalanceAlertEnabled,
                HighBalanceThreshold = userSettings.HighBalanceThreshold,
                IncomeDepositedAlertEnabled = userSettings.IncomeDepositedAlertEnabled,
                DueDateReminderAlertEnabled = userSettings.DueDateReminderAlertEnabled,
                OverspendingAlertEnabled = userSettings.OverspendingAlertEnabled,
                TargetAmountReachedAlertEnabled = userSettings.TargetAmountReachedAlertEnabled
                // Map properties
            };

            return alertSettings;
        }

        private Guid GetCurrentUserId()
        {
            return new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }

        private async Task SaveUserAlertSettings(AlertSettingsViewModel alertSettings)
        {
            //var user = await _userManager.GetUserAsync(User);
            //var userId = user.Id;
            var userId = GetCurrentUserId();

            var userSettings = _db.UserAlertSettings.FirstOrDefault(u => u.UserID == userId);

            if (userSettings == null)
            {
                userSettings = new UserAlertSettings
                {
                    UserID = userId
                };
                _db.UserAlertSettings.Add(userSettings);
            }

            userSettings.LowBalanceAlertEnabled = alertSettings.LowBalanceAlertEnabled;
            userSettings.LowBalanceThreshold = alertSettings.LowBalanceThreshold;
            userSettings.HighBalanceAlertEnabled = alertSettings.HighBalanceAlertEnabled;
            userSettings.HighBalanceThreshold = alertSettings.HighBalanceThreshold;
            userSettings.IncomeDepositedAlertEnabled = alertSettings.IncomeDepositedAlertEnabled;
            userSettings.DueDateReminderAlertEnabled = alertSettings.DueDateReminderAlertEnabled;
            userSettings.OverspendingAlertEnabled = alertSettings.OverspendingAlertEnabled;
            userSettings.TargetAmountReachedAlertEnabled = alertSettings.TargetAmountReachedAlertEnabled;
            // Update properties

            // Save changes to the database.
            await _db.SaveChangesAsync();
            //_logger.LogInformation($"Saved Tunde addition success - {userSettings}");

        }

    }
}
