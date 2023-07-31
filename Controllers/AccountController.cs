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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly HttpClient _client;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _client = clientFactory.CreateClient("MyApiClient");
            // the base address of the mock web API created on ASP .NET Core WEB APIProject
            _client.BaseAddress = new Uri("https://localhost:7078/api/BankA/");
        }

        private async Task<decimal> GetAccountBalanceFromAPI(int accountNumber)
        {
            try
            {
                _logger.LogInformation("Calling API to get account balance...");
                // Call the API to get the account balance
                var response = await _client.GetAsync($"balance/{accountNumber}");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // To parse the response and extract the balance value
                    if (decimal.TryParse(content, out decimal balance))
                    {
                        return balance;
                    }
                }
                else
                {
                    Console.WriteLine("Error occurred while calling API. Status Code: {0}", response?.StatusCode);
                    _logger.LogError("Error occurred while calling API. Status Code: {StatusCode}", response?.StatusCode);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex, "Error occurred while retrieving account balance from API.");
                Console.WriteLine("Error occurred while retrieving account balance from API: {0}", ex.Message);
            }
            // Return 0 if retrieval fails
            return 0;
        }

        private async Task<List<Transaction>> GetTransactionsFromAPI(int accountNumber)
        {
            try
            {
                // Call the API to get the transactions
                var response = await _client.GetAsync($"transactions/{accountNumber}");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Deserialize the response to List<Transaction> and return
                    var transactions = JsonConvert.DeserializeObject<List<Transaction>>(content);
                    Console.WriteLine($"Transactions received as {transactions}");

                    return transactions;
                }
            }
            catch (Exception ex)
            {
                var ErrorMessage = ex.Message;
                TempData["error"] = ErrorMessage;
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex, "Error occurred while retrieving transactions from API.");
                Console.WriteLine($"Error occurred while retrieving transactions from API: {ErrorMessage}", ex.Message);
            }

            // Return an empty list if retrieval fails
            return new List<Transaction>();
        }


        public IActionResult AgreeRetrieveData(int accountNumber)
        {
            var account = new Account { AccountNumber = accountNumber };

            return View(account);
        }


        [HttpPost]
        public async Task<IActionResult> RetrieveData()
        {

            //Retrieve the inputted account number from the form
            if (!int.TryParse(Request.Form["AccountNumber"], out int accountNumber))
            {
                TempData["success"] = "Invalid account number input.";
                return RedirectToAction("CreateAccount");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["success"] = "User not found.";
                return RedirectToAction("CreateAccount");
            }

            var userId = Guid.Parse(user.Id);
            // Check the account with the given account number & if it belongs to the current user
            var account = _db.Accounts.FirstOrDefault(a => a.UserID == userId && a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["success"] = "Account not found for the current user.";
                return RedirectToAction("CreateAccount");
            }

            try
            {
                // Call the API integration method to get account balance
                decimal accountBalance = await GetAccountBalanceFromAPI(accountNumber);

                // Call the API integration method to get transactions
                List<Transaction> transactions = await GetTransactionsFromAPI(accountNumber);

                // Update the Account table with the retrieved balance
                account.Balance = accountBalance;
                _db.SaveChanges();

                // Update the Transaction table with the retrieved transactions 
                foreach (var transaction in transactions)
                {
                    // Set the account ID for the transaction to the account's ID
                    transaction.AccountID = account.AccountID;
                    transaction.UserID = account.UserID;
                    transaction.InstitutionName = account.InstitutionName;
                    transaction.CreatedAt = DateTime.Now;
                    transaction.UpdatedAt = DateTime.Now;

                    // Save the transaction to the Transaction table
                    _db.Transactions.Add(transaction);
                }
                Console.WriteLine($"Transactions saved: {transactions} ");
                _db.SaveChanges();

                TempData["success"] = "Balance and Transactions retrieved successfully.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                // Log exception specific database related errors
                _logger.LogError(ex, "An error occurred while saving changes to the database: {Message}", ex.Message);
                TempData["error"] = "An error occurred while saving changes to the database.";
                return RedirectToAction("CreateAccount");
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while retrieving data from the API: " + ex.Message;
                return RedirectToAction("CreateAccount");
            }

        }


        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = Guid.Parse(user.Id);

                var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();
                ViewBag.Id = userId;

                // Calculate total balance
                decimal totalBalance = accounts.Sum(a => a.Balance);
                ViewBag.TotalBalance = totalBalance;


                // Get the current date and the date 7 days ago
                DateTime currentDate = DateTime.Now;
                DateTime sevenDaysAgo = currentDate.AddDays(-7);

                // Calculate total balance for each day in the last 7 days and the current day
                var dailyBalances = new List<(DateTime Date, decimal Balance)>();
                foreach (var date in EachDay(sevenDaysAgo, currentDate))
                {
                    foreach (var account in accounts)
                    {
                        // Initialize the BalanceForDay dictionary if it's null
                        if (account.BalanceForDay == null)
                        {
                            account.BalanceForDay = new Dictionary<DateTime, decimal>();
                        }

                        // Query the transactions for each account on the current date
                        var transactionsOnDate = _db.Transactions
                            .Where(t => t.AccountID == account.AccountID && t.CreatedAt.Date == date.Date)
                            .ToList();

                        // Calculate the balance for the account on the current date by summing up the transactions
                        decimal balanceOnDate = account.Balance;
                        if (transactionsOnDate.Count > 0)
                        {
                            balanceOnDate += transactionsOnDate.Sum(t => t.Amount);
                        }

                        // Store the balance for the account on the current date
                        account.BalanceForDay[date.Date] = balanceOnDate;
                    }

                    // Calculate the total balance for all accounts on the current date
                    decimal totalBalanceForDay = accounts.Sum(a => a.BalanceForDay.ContainsKey(date.Date) ? a.BalanceForDay[date.Date] : 0);
                    dailyBalances.Add((date, totalBalanceForDay));
                }

                ViewBag.DailyBalances = dailyBalances;

                var accountTypes = new string[] { "CreditCard", "BankAccount", "Investment" };
                var dailyBalancesByAccountType = new Dictionary<string, List<(DateTime Date, decimal Balance)>>();

                foreach (var accountType in accountTypes)
                {
                    // Initialize the dictionary for the current account type
                    dailyBalancesByAccountType[accountType] = new List<(DateTime Date, decimal Balance)>();

                    // Iterate over each day in the last 7 days and calculate the balance for the current account type
                    foreach (var date in EachDay(sevenDaysAgo, currentDate))
                    {
                        // Filter accounts of the current account type
                        var accountsOfType = accounts.Where(a => a.AccountType == accountType).ToList();

                        // Calculate the total balance for the current account type on the current date
                        decimal totalBalanceForDay = accountsOfType.Sum(a =>
                        {
                            // Query the transactions for each account on the current date
                            var transactionsOnDate = _db.Transactions
                                .Where(t => t.AccountID == a.AccountID && t.CreatedAt.Date == date.Date)
                                .ToList();

                            // Calculate the balance for the account on the current date by summing up the transactions
                            decimal balanceOnDate = a.Balance;
                            if (transactionsOnDate.Count > 0)
                            {
                                balanceOnDate += transactionsOnDate.Sum(t => t.Amount);
                            }

                            // Store the balance for the account on the current date
                            a.BalanceForDay[date.Date] = balanceOnDate;

                            return balanceOnDate;
                        });

                        // Add the total balance for the current account type on the current date to the dictionary
                        dailyBalancesByAccountType[accountType].Add((date, totalBalanceForDay));
                    }
                }

                ViewBag.DailyBalancesByAccountType = dailyBalancesByAccountType;



                // Calculate total balance per account type for each day in the last 7 days and the current day
                // Calculate total balance for the current day
                var totalBalanceCurrentDay = accounts.Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.TotalBalanceCurrentDay = totalBalanceCurrentDay;

                // Calculate total balance for the last 7 days
                var totalBalanceLast7Days = accounts.Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.TotalBalanceLast7Days = totalBalanceLast7Days;

                // Calculate total credit card balance for the last 7 days
                var totalCreditCardBalanceLast7Days = accounts
                    .Where(a => a.AccountType == "CreditCard")
                    .Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.TotalCreditCardBalanceLast7Days = totalCreditCardBalanceLast7Days;

                // Calculate total bank account balance for the last 7 days
                var totalBankAccountBalanceLast7Days = accounts
                    .Where(a => a.AccountType == "BankAccount")
                    .Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.TotalBankAccountBalanceLast7Days = totalBankAccountBalanceLast7Days;

                // Calculate total investment balance for the last 7 days
                var totalInvestmentBalanceLast7Days = accounts
                    .Where(a => a.AccountType == "Investment")
                    .Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.TotalInvestmentBalanceLast7Days = totalInvestmentBalanceLast7Days;

                // Calculate total credit card balance, bank account balance, and investment balance for the current day
                decimal creditCardBalanceCurrentDay = accounts
                    .Where(a => a.AccountType == "CreditCard")
                    .Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.CreditCardBalanceCurrentDay = creditCardBalanceCurrentDay;

                decimal bankAccountBalanceCurrentDay = accounts
                    .Where(a => a.AccountType == "BankAccount")
                    .Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.BankAccountBalanceCurrentDay = bankAccountBalanceCurrentDay;

                decimal investmentBalanceCurrentDay = accounts
                    .Where(a => a.AccountType == "Investment")
                    .Sum(a => a.BalanceForDay.ContainsKey(currentDate.Date) ? a.BalanceForDay[currentDate.Date] : 0);
                ViewBag.InvestmentBalanceCurrentDay = investmentBalanceCurrentDay;


                return View(accounts);
            }
            return RedirectToAction("Index");
        }


        // Helper method to get each day between two dates(inclusive)
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        public async Task<IActionResult> CreateAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = Guid.Parse(user.Id);
            if (user != null)
            {
                Account obj = new Account();
                obj.UserID = userId;

                //var userId = user.Id;
                ViewBag.Id = obj.UserID;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount(Account obj, string button)
        {
            try
            {
                if (button == "Submit") // Check which button was clicked
                {
                    if (ModelState.IsValid)
                    {
                        obj.CreatedAt = DateTime.Now;
                        obj.UpdatedAt = DateTime.Now;

                        // Retrieve the current user
                        var user = _userManager.GetUserAsync(User).Result;

                        // Establish the association between the account and user
                        var id = obj.UserID;

                        _db.Accounts.Add(obj);
                        _db.SaveChanges();
                        TempData["success"] = "Account created successfully";

                        return RedirectToAction("Index");
                    }

                    else
                    {
                        TempData["error"] = "Invalid input. Please correct the errors below.";
                        ViewBag.Id = obj.UserID;
                        return View(obj);
                    }
                }

                else if (button == "Validate")
                {

                    obj.CreatedAt = DateTime.Now;
                    obj.UpdatedAt = DateTime.Now;

                    var id = obj.UserID;

                    // Retrieve the current user
                    var user = _userManager.GetUserAsync(User).Result;


                    _db.Accounts.Add(obj);
                    _db.SaveChanges();
                    //TempData["success"] = "Validation successful";
                    TempData["success"] = "Account saved successfully";

                    // Redirect to AgreeRetrieveData action with the account number as a query parameter
                    return RedirectToAction("AgreeRetrieveData", new { accountNumber = obj.AccountNumber });

                }
                else
                {
                    // Invalid button value, handle accordingly
                    TempData["error"] = "Invalid input. Please correct the errors below.";
                    ViewBag.Id = obj.UserID;
                    return View(obj);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an account.");
                //_logger.Log(ex);
                return BadRequest();
            }
        }



        [HttpGet]
        public IActionResult DeleteAccount(int AccountID)
        {
            // Retrieve the account based on the provided AccountID
            var account = _db.Accounts.FirstOrDefault(a => a.AccountID == AccountID);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int AccountID)
        {
            // Retrieve the account based on the provided AccountID
            var account = _db.Accounts.FirstOrDefault(a => a.AccountID == AccountID);

            if (account == null)
            {
                return NotFound();
            }

            _db.Accounts.Remove(account);
            _db.SaveChanges();

            TempData["success"] = "Account deleted successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditAccount(int AccountID)
        {
            // Retrieve the account based on the provided AccountID
            //var account = _db.Accounts.FirstOrDefault(a => a.AccountID == AccountID);
            var account = _db.Accounts.FirstOrDefault(a => a.AccountID == AccountID);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        public IActionResult UpdateAccount(Account updatedAccount, string button)
        {
            if (button == "Submit") // Check which button was clicked
            {

                if (ModelState.IsValid)
                {
                    // Retrieve the original account from the database
                    var account = _db.Accounts.FirstOrDefault(a => a.AccountID == updatedAccount.AccountID);

                    if (account == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the account with the values from the updated account
                    account.syncType = updatedAccount.syncType;
                    account.AccountType = updatedAccount.AccountType;
                    account.AccountNumber = updatedAccount.AccountNumber;
                    account.InstitutionName = updatedAccount.InstitutionName;
                    account.Balance = updatedAccount.Balance;
                    account.UpdatedAt = DateTime.Now;

                    _db.SaveChanges();

                    TempData["success"] = "Account updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Invalid input. Please correct the errors below.";
                    return View("EditAccount", updatedAccount);
                }
            }
            else if (button == "Validate")
            {
                if (ModelState.IsValid)
                {
                    // Retrieve the original account from the database
                    var account = _db.Accounts.FirstOrDefault(a => a.AccountID == updatedAccount.AccountID);

                    if (account == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the account with the values from the updated account
                    account.syncType = updatedAccount.syncType;
                    account.AccountType = updatedAccount.AccountType;
                    account.AccountNumber = updatedAccount.AccountNumber;
                    account.InstitutionName = updatedAccount.InstitutionName;
                    account.Balance = updatedAccount.Balance;
                    account.UpdatedAt = DateTime.Now;

                    _db.SaveChanges();

                    TempData["success"] = "Account updated successfully";

                    // Redirect to AgreeRetrieveData action with the account number as a query parameter
                    return RedirectToAction("AgreeRetrieveData", new { accountNumber = updatedAccount.AccountNumber });
                }
                else
                {
                    // Invalid button value, handle accordingly
                    TempData["error"] = "Invalid input. Please correct the errors below.";
                    return View("EditAccount", updatedAccount);
                }
            }

            return View(updatedAccount);
        }



    }
}

