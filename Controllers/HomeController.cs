﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinanceChecker.Models;
using Microsoft.AspNetCore.Identity;
using FinanceChecker.DataTransferObject;

namespace FinanceChecker.Controllers;

public class HomeController : Controller

{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;


    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
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
                ViewBag.UserId = userId;
            }
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && user is ApplicationUser applicationUser && SecurityQuestionAndAnswerMatches(applicationUser, model.SecurityQuestion, model.SecurityAnswer))
        {
            // If the security question and answer are valid, redirect to the ResetPassword action.
            return RedirectToAction("ResetPassword", new { userId = user.Id });
        }

        // If security question/answer is invalid, display an error message.
        ModelState.AddModelError("", "Invalid security question/answer.");
        TempData["error"] = "Invalid security question/answer";

        return View();
    }

    // Helper method to check if the security question and answer match.
    private bool SecurityQuestionAndAnswerMatches(ApplicationUser user, string question, string answer)
    {
        return user.SecurityQuestion == question && user.SecurityAnswer == answer;
    }



    [HttpGet]
    public IActionResult ResetPassword(string userId)
    {
    
        var model = new ResetPasswordViewModel
        {
            UserId = userId
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user != null)
        {
            // Reset the user's password
            var resetPasswordResult = await _userManager.RemovePasswordAsync(user);
            if (resetPasswordResult.Succeeded)
            {
                var setPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (setPasswordResult.Succeeded)
                {
                    // Password reset success logic.
                    return RedirectToAction("PasswordResetSuccess");
                }
                else
                {
                    // Failed to set new password. Display error messages.
                    foreach (var error in setPasswordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                // Failed to remove old password. Display error messages.
                foreach (var error in resetPasswordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    var descr = error.Description;
                    TempData["error"] = descr;

                }
            }
        }

        return View(model);
    }

    public IActionResult PasswordResetSuccess()
    {
        return View();
    }

    public IActionResult PasswordResetError()
    {
        return View();
    }


}