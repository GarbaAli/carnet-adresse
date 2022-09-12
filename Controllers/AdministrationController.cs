using carnet_adresse.Models;
using carnet_adresse.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdministrationCreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };

                IdentityResult  result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Administration");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        public IActionResult List()
        {
            var rols = _roleManager.Roles;
            return View(rols);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var rol = await _roleManager.FindByIdAsync(id);
            if (rol is null)
            {
                return NotFound();
            }
               
            AdministrationEditRoleViewModel model = new AdministrationEditRoleViewModel()
            {
                Id = rol.Id,
                RoleName  = rol.Name,
                Users = new List<string>()
            };
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, model.RoleName))
                {
                    model.Users.Add(user.Email);
                }
            }
             return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdministrationEditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role is null)
                {
                    return NotFound();
                }
                role.Name = model.RoleName;
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddRemoveUserRole(string RoleId)
        {
            if (!string.IsNullOrEmpty(RoleId))
            {
                var role = await _roleManager.FindByIdAsync(RoleId);
                if (role != null)
                {
                    List<AdministrationAddRemoveUserRole> models = new List<AdministrationAddRemoveUserRole>();
                    foreach (AppUser user in _userManager.Users)
                    {
                        AdministrationAddRemoveUserRole model = new AdministrationAddRemoveUserRole();
                        model.UserId = user.Id;
                        model.UserName = user.UserName;
                        model.IsSelected = false;

                        if (await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            model.IsSelected = true;
                        }
                        models.Add(model);
                    }
                    ViewBag.Role = RoleId;
                    ViewBag.Name = role.Name;
                    return View(models);
                }
            }
            return RedirectToAction("List", "Administration");
        }

        [HttpPost]
        public async Task<IActionResult> AddRemoveUserRole(List<AdministrationAddRemoveUserRole> model, string RoleId)
        {
                var role = await _roleManager.FindByIdAsync(RoleId);
                if (role != null)
                {
                    IdentityResult result = null;
                    for (int i = 0; i < model.Count; i++)
                    {
                        AppUser user = await _userManager.FindByIdAsync(model[i].UserId);
                        if (await _userManager.IsInRoleAsync(user, role.Name) && !model[i].IsSelected)
                        {
                           result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                        else if(!await _userManager.IsInRoleAsync(user, role.Name) && model[i].IsSelected)
                        {
                            result = await _userManager.AddToRoleAsync(user, role.Name);
                        }
                    }

                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }
                    }
                }
            return RedirectToAction("Edit", new { id = RoleId  });
        }
    }
}
