using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]

    public class SavingsController : Controller
    {
        private readonly ILogger<SavingsController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public SavingsController(ILogger<SavingsController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            // Retrieve savings goals for the current user from the database
            var user = await _userManager.GetUserAsync(User);
            var userId = GetCurrentUserId();
            if (user != null)
            {
                var savingsGoals = _db.Savings.Where(s => s.UserID == userId).ToList();
                return View(savingsGoals);
            }

            return View();
        }

        private Guid GetCurrentUserId()
        {
            return new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }


            [HttpGet]
        public IActionResult CreateSavings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSavings(Savings savings)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = GetCurrentUserId();

                if (user != null)
                {
                    savings.UserID = userId;
                    savings.CreatedAt = DateTime.Now;
                    savings.UpdatedAt = DateTime.Now;


                    // Calculate progress as a percentage of CurrentSavings to Goal
                    if (savings.Goal > 0)
                    {
                        savings.Progress = (savings.CurrentSavings / savings.Goal) * 100;
                    }
                    else
                    {
                        savings.Progress = 0;
                    }

                    _db.Savings.Add(savings);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View("CreateSavings", savings);
        }

        [HttpGet]
        public IActionResult EditSavings(int id)
        {
            // Retrieve the savings goal from the database using the provided id
            var savings = _db.Savings.FirstOrDefault(s => s.SavingsID == id);

            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        [HttpPost]
        public IActionResult EditSavings(Savings updatedSavings)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the original savings goal from the database using the provided id
                var originalSavings = _db.Savings.FirstOrDefault
                    (s => s.SavingsID == updatedSavings.SavingsID);

                if (originalSavings == null)
                {
                    return NotFound();
                }

                // Update the properties of the original savings goal
                originalSavings.Goal = updatedSavings.Goal;
                originalSavings.SavingsName = updatedSavings.SavingsName;
                originalSavings.DueDate = updatedSavings.DueDate;
                originalSavings.CurrentSavings = updatedSavings.CurrentSavings;
                originalSavings.UpdatedAt = DateTime.Now;

                // Calculate progress as a percentage of CurrentSavings to Goal
                if (originalSavings.Goal > 0)
                {
                    originalSavings.Progress = (originalSavings.CurrentSavings / originalSavings.Goal) * 100;
                }
                else
                {
                    originalSavings.Progress = 0;
                }

                // Update the savings goal in the database
                _db.Savings.Update(originalSavings);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If ModelState is not valid, calculate progress based on the current and goal savings
            if (updatedSavings.Goal > 0)
            {
                updatedSavings.Progress = (updatedSavings.CurrentSavings / updatedSavings.Goal) * 100;
            }
            else
            {
                updatedSavings.Progress = 0;
            }

            return View(updatedSavings);
        }


        public IActionResult DeleteSavings(int id)
        {
            // Retrieve the savings goal from the database using the provided id
            var savings = _db.Savings.FirstOrDefault(s => s.SavingsID == id);

            if (savings == null)
            {
                return NotFound();
            }

            _db.Savings.Remove(savings);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
