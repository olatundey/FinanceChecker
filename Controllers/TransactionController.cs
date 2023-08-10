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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    [Authorize]
    //    [AutoValidateAntiforgeryToken]
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public TransactionController(ILogger<TransactionController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }


        public async Task<IActionResult> Transaction()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = Guid.Parse(user.Id);
            if (user != null)
            {
                //var userId = user.Id;
                var accounts = _db.Accounts.Where(a => a.UserID == userId).ToList();
                ViewBag.Id = userId;

                // Retrieve the transaction history for the current user
                var transactions = _db.Transactions.Where(t => t.UserID == userId).ToList();
                return View(transactions); // Pass the transactions as the model
            }

            return View();
        }

        [Route("Transaction/GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = Guid.Parse(user.Id);
            if (user != null)
            {
                //var userId = user.Id;
                var transactions = _db.Transactions.Where(t => t.UserID == userId).ToList();
                return Json(transactions);
            }

            return Json(null);
        }

        // GET: Account/CreateTransaction
        public async Task<IActionResult> CreateTransaction()
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
            //pass category and accounts to view
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
                var userId = Guid.Parse(user.Id);

                obje.UserID = userId;
                obje.CreatedAt = DateTime.Now;
                obje.UpdatedAt = DateTime.Now;

                _db.Transactions.Add(obje);
                Console.WriteLine(obje);
                _db.SaveChanges();

                // Retrieve the corresponding account for the transaction
                var account = _db.Accounts.FirstOrDefault(a => a.AccountID == obje.AccountID);

                if (account != null)
                {
                    // Update the account balance based on the added transaction
                    account.Balance += obje.Amount; // Assuming Amount is the transaction amount
                    _db.SaveChanges();
                }
                TempData["success"] = "Transaction created successfully";
                return RedirectToAction("Transaction");
            }

            // Repopulate ViewBag data 
            var accounts = _db.Accounts.Where(a => a.UserID == obje.UserID).ToList();

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

           
            // Check and set TempData for each empty or unselected field
            if (string.IsNullOrEmpty(obje.InstitutionName))
            {
                TempData["InstitutionNameInfo"] = "Institution Name is required.";
            }

            if (obje.Amount == 0)
            {
                TempData["AmountInfo"] = "Amount is required.";
            }

            if (string.IsNullOrEmpty(obje.Category))
            {
                TempData["CategoryInfo"] = "Category is required.";
            }

            if (string.IsNullOrEmpty(obje.Description))
            {
                TempData["DescriptionInfo"] = "Description is required.";
            }

            if (obje.Date == null || obje.Date == DateTime.MinValue)
            {
                TempData["DateInfo"] = "Date is required.";
            }
            return View(obje);
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
            var userId = Guid.Parse(user.Id);
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
                    var account = _db.Accounts.FirstOrDefault(a => a.AccountID == existingTransaction.AccountID);

                    if (account != null)
                    {
                        // Update the account balance based on the added transaction
                        account.Balance += existingTransaction.Amount; // Assuming Amount is the transaction amount
                        _db.SaveChanges();
                    }
                    TempData["success"] = "Transaction Updated successfully";
                    return RedirectToAction("Transaction");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

          

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

             var account = _db.Accounts.FirstOrDefault(a => a.AccountID == transaction.AccountID);

                    if (account != null)
                    {
                        // Update the account balance based on the added transaction
                        account.Balance += transaction.Amount; // Assuming Amount is the transaction amount
                        _db.SaveChanges();
                    }
            TempData["success"] = "Transaction deleted successfully";
            return RedirectToAction("Transaction");
        }


    }
}

