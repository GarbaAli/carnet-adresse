using carnet_adresse.Models;
using carnet_adresse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Controllers
{
    [AllowAnonymous]       
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }
        [HttpGet]
        public IActionResult Register()  
        {
            return View();
        }

        public async Task<IActionResult> Register(AccountRegisterViewModel model)
        {
            if (ModelState.IsValid)   
            {
                AppUser user = new AppUser
                {
                    //UserName = GenerateUserName(model.FirstName, model.LastName),
                    FirstName = model.FirstName,
                    LasttName = model.LastName,
                    Age = model.Age,
                    UserName = model.Email,
                    Email = model.Email
                };
               var result = await  _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                   await _signManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Contact");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Contact");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
                        return RedirectToAction("Index", "Contact");
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                ModelState.AddModelError(string.Empty, "Login Invalid Attempt");
            }
            return View(model);
        }

        private string GenerateUserName(string FirstName, string LastName)
        {
            return FirstName.Trim().ToUpper() + "_" + LastName.Trim().ToLower();
        }

        [AcceptVerbs("Get", "post")]
        public async Task<IActionResult> CheckingExistingEMail(AccountRegisterViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"This Email {model.Email} is already in use");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    AccountEditViewModel model = new AccountEditViewModel()
                    {
                        FirstName = user.FirstName,
                        LasttName = user.LasttName,
                        Age = user.Age,
                        Password = user.PasswordHash,
                        ConfirmPassword = user.PasswordHash
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Contact");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByIdAsync(model.Id);
                
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        //Acher le password
                        var passwordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                        if (passwordHash != user.PasswordHash)
                        {
                            user.FirstName = model.FirstName;
                            user.LasttName = model.LasttName;
                            user.Age = model.Age;
                            user.PasswordHash = passwordHash;
                        }
                        
                    }
                    else
                    {
                        user.FirstName = model.FirstName;
                        user.LasttName = model.LasttName;
                        user.Age = model.Age;
                    }
                    

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Detail", new { id  = model.Id });
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

              
            }

            return View(model);
        }

            public async Task<IActionResult> Detail(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index", "Contact");
        }
    }
}
