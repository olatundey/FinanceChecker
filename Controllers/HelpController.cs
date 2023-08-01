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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public IActionResult FAQs()
        {

            var faq = _db.FAQs.ToList();

            return View(faq);
        }

        public IActionResult VideoTutorials()
        {
            // Fetch the list of video tutorials from the database
            List<Videos> tutorials = _db.AppVideo.ToList();
            return View(tutorials);
        }

    public IActionResult Contact()
        {
            var model = new ContactForm(); // Initialize the ContactForm model
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitContactForm(ContactForm form)
        {
            // Validate the form data
            if (ModelState.IsValid)
            {
                // Map the ContactForm to the ContactUS model
                var contactUsModel = new ContactForm
                {
                    Name = form.Name,
                    Email = form.Email,
                    Message = form.Message
                };

                // Save the form data to the ContactUS table
                _db.ContactUs.Add(contactUsModel);
                _db.SaveChanges();

                // Addition, Process the form data, send email to support team,email sending logic
                return RedirectToAction("ThankYou");
            }

            // If the form data is not valid, return to the contact form view with validation errors
            return View("Contact", form);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}