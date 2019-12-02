using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFridgeApp.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        //    private readonly UserManager<ApplicationUser> _userManager;
        //    private readonly SignInManager<ApplicationUser> _signInManager;

        //    public AccountController(
        //        UserManager<ApplicationUser> userManager,
        //        SignInManager<ApplicationUser> signInManager
        //       )
        //    {
        //        _userManager = userManager;
        //        _signInManager = signInManager;
        //    }

        //    [TempData]
        //    public string ErrorMessage { get; set; }

        //    [HttpGet]
        //    [AllowAnonymous]
        //    public async Task<IActionResult> Login(string returnUrl = null)
        //    {
        //        // Clear the existing external cookie to ensure a clean login process
        //        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        //        return View();
        //    }

        //    [HttpGet]
        //    [AllowAnonymous]
        //    public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        //    {
        //        // Ensure the user has gone through the username & password screen first
        //        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //        if (user == null)
        //        {
        //            throw new ApplicationException($"Unable to load two-factor authentication user.");
        //        }

        //        ViewData["ReturnUrl"] = returnUrl;

        //        return View();
        //    }

        //    [HttpGet]
        //    [AllowAnonymous]
        //    public IActionResult Lockout()
        //    {
        //        return View();
        //    }

        //    [HttpGet]
        //    [AllowAnonymous]
        //    public IActionResult Register(string returnUrl = null)
        //    {
        //        ViewData["ReturnUrl"] = returnUrl;
        //        return View();
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Logout()
        //    {
        //        await _signInManager.SignOutAsync();
        //        return Ok();
        //    }

        //    [HttpGet]
        //    public IActionResult AccessDenied()
        //    {
        //        return View();
        //    }
        //}
    }
}
