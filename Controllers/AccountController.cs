using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.DataTransferObject;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

            public async Task<IActionResult> Index()
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var userId = user.Id;
                        var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();
                        ViewBag.Id = userId;

                        // Calculate total balance
                        decimal totalBalance = accounts.Sum(a => a.Balance);
                        ViewBag.TotalBalance = totalBalance;

                        return View(accounts);
                    }
                }

                return RedirectToAction("Index"); // Redirect to the login page if the user is not authenticated or if the user object is null
            }

            //public IActionResult AddAccount()
            //{
            //    return View();
            //}
            public async Task<IActionResult> CreateAccount()
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        Account obj = new Account();
                        obj.UserID = user.Id;

                        //var userId = user.Id;
                        ViewBag.Id = obj.UserID;
                    }
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
                            return View(obj);

                        }
                    }

                    else if (button == "Validate")
                    {
                    // Perform validation logic for the validate button

                    //// Example validation code: check if the account number is empty
                    //if (obj.AccountNumber == 0)
                    //{
                    //    ModelState.AddModelError(string.Empty, "Account Number expected with Numbers");                       
                    //    TempData["error"] = "Account Number expected with Numbers";
                    //    return View(obj);
                    //}

                    obj.CreatedAt = DateTime.Now;
                        obj.UpdatedAt = DateTime.Now;

                        var id = obj.UserID;

                        // Retrieve the current user
                        var user = _userManager.GetUserAsync(User).Result;

                        // Additional validation logic...

                        // If validation passes, display success message
                        _db.Accounts.Add(obj);
                        _db.SaveChanges();
                        TempData["success"] = "Validation successful";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Invalid button value, handle accordingly
                        //return BadRequest();
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
            [ValidateAntiForgeryToken]
            public IActionResult UpdateAccount(Account updatedAccount)
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

                return View(updatedAccount);
            }


            // GET: Account/CreateTransaction
            public async Task<IActionResult> CreateTransaction()
            {
                if (User.Identity.IsAuthenticated)
                {
                    var id = await _userManager.GetUserAsync(User);
                    if (id != null)
                    {
                        Account obj = new Account();
                        obj.UserID = id.Id;

                        //var userId = user.Id;
                        ViewBag.Id = obj.UserID;
                    }
                }

                var user = _userManager.GetUserAsync(User).Result;
                var userId = user.Id;
                var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();

                var accountViewModels = new List<AccountViewModel>();

                foreach (var account in accounts)
                {
                    var accountViewModel = new AccountViewModel
                    {
                        AccountID = account.AccountID,
                        InstitutionName = account.InstitutionName
                    };
                    accountViewModels.Add(accountViewModel);
                }
                var categories = _db.Categories.ToList();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
                ViewBag.Accounts = accountViewModels;

                return View(new Transaction());
            }

            [HttpPost]
            public IActionResult CreateTransaction(Transaction obje)
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.GetUserAsync(User).Result;
                    obje.UserID = user.Id;
                    obje.CreatedAt = DateTime.Now;
                    obje.UpdatedAt = DateTime.Now;

                    _db.Transactions.Add(obje);
                    _db.SaveChanges();
                TempData["success"] = "Transaction created successfully";
                return RedirectToAction("Transaction");
                }


                return View(obje);
            }


        public async Task<IActionResult> Transaction()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = user.Id;
                var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();
                ViewBag.Id = userId;

                // Retrieve the transaction history for the current user
                var transactions = _db.Transactions.Where(t => t.UserID == userId).ToList();
                return View(transactions); // Pass the transactions as the model
            }

            return View();
        }

        [Route("Account/GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = user.Id;
                var transactions = _db.Transactions.Where(t => t.UserID == userId).ToList();
                return Json(transactions);
            }

            return Json(null);
        }




        // GET: Transaction/Edit/5
        public IActionResult EditTransaction(int TransactionID)
            {
                var transaction = _db.Transactions.Find(TransactionID);
                if (transaction == null)
                {
                    return NotFound();
                }
                var user = _userManager.GetUserAsync(User).Result;
                var userId = user.Id;
                var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();

                var accountViewModels = new List<AccountViewModel>();

                foreach (var account in accounts)
                {
                    var accountViewModel = new AccountViewModel
                    {
                        AccountID = account.AccountID,
                        InstitutionName = account.InstitutionName
                    };
                    accountViewModels.Add(accountViewModel);
                }

                // Populate the ViewBag.Categories
                var categories = _db.Categories.ToList();
                ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");
                ViewBag.Accounts = accountViewModels;

            return View("EditTransaction", transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTransaction(int TransactionID, Transaction transaction)
        {
            if (TransactionID != transaction.TransactionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTransaction = _db.Transactions.Find(TransactionID);
                    if (existingTransaction == null)
                    {
                        return NotFound();
                    }

                    existingTransaction.AccountID = transaction.AccountID;
                    existingTransaction.InstitutionName = transaction.InstitutionName;
                    existingTransaction.Amount = transaction.Amount;
                    existingTransaction.Category = transaction.Category;
                    existingTransaction.Description = transaction.Description;
                    existingTransaction.Date = transaction.Date;
                    existingTransaction.UpdatedAt = DateTime.Now;

                    _db.Transactions.Update(existingTransaction);
                    _db.SaveChanges();
                    TempData["success"] = "Transaction Updated successfully";
                    return RedirectToAction("Transaction");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            //// Populate the ViewBag.Categories
            //var categories = _db.Categories.ToList();
            //ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");

            return View("EditTransaction", transaction);
        }

        // GET: Transaction/Delete/5
        public IActionResult DeleteTransaction(int TransactionID)
            {
                var transaction = _db.Transactions.Find(TransactionID);
                if (transaction == null)
                {
                    return NotFound();
                }

                return View(transaction);
            }

            // POST: Account/ConfirmDeleteTransaction/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            [ActionName("ConfirmDeleteTransaction")]
            public IActionResult ConfirmDeleteTransaction(int TransactionID)
            {
                var transaction = _db.Transactions.FirstOrDefault(a => a.TransactionID == TransactionID);
                if (transaction == null)
                {
                    return NotFound();
                }

                _db.Transactions.Remove(transaction);
                _db.SaveChanges();
                TempData["success"] = "Transaction deleted successfully";

                return RedirectToAction("Transaction");
            }


        }
    }

