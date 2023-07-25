using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly ILogger<BudgetController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public BudgetController(ILogger<BudgetController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }


        public async Task<IActionResult> AddCategory()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = user.Id;
                ViewBag.Id = userId;

                return View();
            }

            return RedirectToAction("Index"); 
        }


        // POST: Category/Create
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("AddCategory", category);
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = new Guid(user.Id); // Convert user.Id to Guid

            if (user != null)
            {
                //var userId = user.Id;
                var budgets = _db.Budgets.Where(b => b.UserID == userId).ToList();

                // Recalculate total transactions amount for each budget category
                foreach (var budget in budgets)
                {
                    var transactionsForCategory = _db.Transactions.Where
                        (t => t.Category == budget.CategoryName && t.UserID == user.Id
                        );
                    budget.TotalTransactionsAmount = transactionsForCategory.Sum(t => t.Amount);

                    if (budget.Amount > 0)
                    {
                        budget.Progress = (budget.TotalTransactionsAmount / budget.Amount) * 100;
                    }
                    else
                    {
                        budget.Progress = 0;
                    }
                }

                return View(budgets);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateBudget()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = new Guid(user.Id); // Convert user.Id to Guid
                var budgets = _db.Budgets.Where(b => b.UserID == userId).ToList();
                var categories = _db.Categories.ToList();

                var budget = new Budget
                {
                    UserID = userId, 
                };

                ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName"); // Use ViewBag to pass the SelectList

                return View(budget);
            }

            return View(new Budget()); // Return an empty Budget model if user is null
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget(Budget budget)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = new Guid(user.Id); // Convert user.Id to Guid
            var existingBudget = _db.Budgets.FirstOrDefault(b => b.UserID == userId && b.CategoryName == budget.CategoryName);

            if (existingBudget != null)
            {
                ModelState.AddModelError("Budget.CategoryName", "Budget category already exists for this user.");
                var existingCategories = _db.Categories.ToList();
                ViewBag.Categories = new SelectList(existingCategories, "CategoryName", "CategoryName");
                TempData["success"] = "Budget category already exists for this user";
                return View(budget);
            }

            if (ModelState.IsValid)
            {
                budget.CreatedAt = DateTime.Now;
                budget.UpdatedAt = DateTime.Now;

                budget.Progress = 0;

                _db.Budgets.Add(budget);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            var budgets = _db.Budgets.Where(b => b.UserID == userId).ToList();
            var categories = _db.Categories.ToList();

            foreach (var b in budgets)
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                var transactionsForCategory = _db.Transactions
                    .Where(t => t.Category == b.CategoryName && t.UserID == user.Id &&
                                t.Date.Month == currentMonth && t.Date.Year == currentYear);

              

                b.TotalTransactionsAmount = transactionsForCategory.Sum(t => Math.Abs(t.Amount));

                if (Math.Abs(b.Amount) > 0)
                {
                    b.Progress = (b.TotalTransactionsAmount / Math.Abs(b.Amount)) * 100;
                }
                else
                {
                    b.Progress = 0;
                }
            }

            ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");

            return View(budget);
        }


        public IActionResult EditBudget(int id)
        {
            var budget = _db.Budgets.FirstOrDefault(b => b.BudgetID == id);

            if (budget == null)
            {
                return NotFound();
            }

            var categories = _db.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");

            return View("EditBudget", budget);
        }

        [HttpPost]
        public async Task<IActionResult> EditBudget(Budget budget)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = new Guid(user.Id); // Convert user.Id to Guid
            var existingBudget = _db.Budgets.FirstOrDefault(b => b.BudgetID != budget.BudgetID && b.UserID == userId && b.CategoryName == budget.CategoryName);

            if (existingBudget != null)
            {
                ModelState.AddModelError("Budget.CategoryName", "Budget category already exists for this user.");
                var existingCategories = _db.Categories.ToList();
                ViewBag.Categories = new SelectList(existingCategories, "CategoryName", "CategoryName");
                TempData["success"] = "Budget category already exists for this user";
                return View(budget);
            }

            if (ModelState.IsValid)
            {
                budget.UpdatedAt = DateTime.Now;

             
                if (Math.Abs(budget.Amount) > 0)
                {
                    budget.Progress = (budget.TotalTransactionsAmount / Math.Abs(budget.Amount)) * 100;
                }
                else
                {
                    budget.Progress = 0;
                }

                _db.Budgets.Update(budget);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            var categories = _db.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryName", "CategoryName");

            return View("EditBudget", budget);
        }


        public IActionResult DeleteBudget(int id)
        {
            var budget = _db.Budgets.FirstOrDefault(b => b.BudgetID == id);

            if (budget == null)
            {
                return NotFound();
            }

            _db.Budgets.Remove(budget);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }

}
