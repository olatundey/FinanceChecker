using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceChecker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceChecker.Controllers
{
    public class HelpController : Controller
    {
            private readonly ILogger<HelpController> _logger;
            private readonly ApplicationDbContext _db;
            private readonly UserManager<IdentityUser> _userManager;

            public HelpController(ILogger<HelpController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
            {
                _logger = logger;
                _db = db;
                _userManager = userManager;
            }

            public IActionResult Index()
            {
                return View();
            }

        }
    }

