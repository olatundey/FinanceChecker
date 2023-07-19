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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddCategory()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var userId = user.Id;
                    ViewBag.Id = userId;

                    return View();
                }
            }

            return RedirectToAction("Index"); // Redirect to the login page if the user is not authenticated or if the user object is null
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
    }


}
