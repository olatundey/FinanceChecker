using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Transaction()
        {
            return View();
        }

        public IActionResult AddTransaction()
        {
            return View();
        }


        public IActionResult AddAccount()
        {
            return View();
        }
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
                    // validation logic for the validate button

                    //check if the account number is empty
                    //if (obj.AccountNumber == 0)

                    //{
                    //    ModelState.AddModelError(string.Empty, "Account Number expected with Numbers");
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

                // Update the properties of the account with the values
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



    }
}

