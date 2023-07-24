using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    [Authorize]
    public class BillsController : Controller
    {
        private readonly ILogger<BillsController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public BillsController(ILogger<BillsController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        // GET: /Bill/Index
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var bills = await _db.Bills.Where(b => b.UserID == userId).ToListAsync();
            return View(bills);
        }

        // GET: /Bill/Create
        public IActionResult CreateBill()
        {
            return View();
        }

        // POST: /Bill/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBill(Bill bill)
        {
            if (ModelState.IsValid)
            {
                bill.UserID = GetCurrentUserId();
                bill.CreatedAt = DateTime.Now;
                bill.UpdatedAt = DateTime.Now;

                _db.Bills.Add(bill);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bill);
        }

   
        public async Task<IActionResult> EditBill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            var bill = await _db.Bills.FirstOrDefaultAsync(b => b.BillID == id && b.UserID == userId);

            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBill(int id, Bill bill)
        {
            if (id != bill.BillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                var existingBill = await _db.Bills.FirstOrDefaultAsync(b => b.BillID == id && b.UserID == userId);

                if (existingBill == null)
                {
                    return NotFound();
                }

                existingBill.BillName = bill.BillName;
                existingBill.Description = bill.Description;
                existingBill.Amount = bill.Amount;
                existingBill.DueDate = bill.DueDate;
                existingBill.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bill);
        }

       
        public async Task<IActionResult> DeleteBill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            var bill = await _db.Bills.FirstOrDefaultAsync(b => b.BillID == id && b.UserID == userId);

            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var userId = GetCurrentUserId();
            var bill = await _db.Bills.FirstOrDefaultAsync(b => b.BillID == id && b.UserID == userId);

            if (bill == null)
            {
                return NotFound();
            }

            _db.Bills.Remove(bill);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private Guid GetCurrentUserId()
        {
            return new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }

        public async Task<IActionResult> GetBillEvents()
        {
            var userId = GetCurrentUserId();
            var bills = await _db.Bills.Where(b => b.UserID == userId).ToListAsync();

            var events = bills.Select(b => new
            {
                title = b.BillName,
                start = b.DueDate.ToString("yyyy-MM-dd"),
                color = "blue"
            }).ToList();

            return Json(events);
        }

    }
}

