﻿using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = _mapper.Map<ApplicationUser>(model);
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(SignIn));
                        foreach (var Error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, Error.Description);

                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "User Is Already Exist ):");

            }
            return View(model);
        }

        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (Flag)
                    {
                        var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (Result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");

            }
            return View();
        }
        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();

        }
        public async Task<IActionResult> SendResetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    //Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //Create Url Which Send in the body of email
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    //Create Email(Email Class,Method To Create This Email)
                    var email = new Email()
                    {
                        Subject = "Reset Your password",
                        Recipients = model.Email,
                        Body = url

                    };

                    //Send Email
                    EmailSetting.SendEmail(email);
                    //Redirect To Action
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(nameof(ForgetPassword), model);

        }
        public IActionResult CheckYourInbox()
        {
            return View();

        }
        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"]=email;
            TempData["token"]=token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var email = TempData["email"] as string;
            var token = TempData["token"] as string;
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
                ModelState.AddModelError(string.Empty, "Invalid Reset Password :(");
            }
            return View(model);
        }
        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
