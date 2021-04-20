using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DbBicyclesLab.ViewModels;
using DbBicyclesLab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DbBicyclesLab.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Delete(string userId)
        {
            User model = await _userManager.FindByIdAsync(userId);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
                await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
