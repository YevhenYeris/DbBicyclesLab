using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using DbBicyclesLab.ViewModels;
using DbBicyclesLab.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MailKit;
using DbBicyclesLab.Utils;
using Microsoft.AspNetCore.Authorization;

namespace DbBicyclesLab.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IdentityContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext identityContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = identityContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && ValidateYear(model.Year))
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRolesAsync(user, new List<string>{ "user" });
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Підтвердження реєстрації",
                        $"Будь ласка, підтвердіть реєстрацію, перейшовши за посиланням: <a href='{callbackUrl}'>link</a>");

                    return Content("Перевірте вказану пошту!");
                    /*await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");*/
                }
                else
                {
                    var err = new IdentityError();
                    err.Description = "Рік некоректний!";
                    result.Errors.Append(err);
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginVewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = (from u in _context.Users where u.UserName == model.Email || u.Email == model.Email select u.UserName).ToList();
                string name = users.Any() ? users.First().ToString() : "";
                var result = 
                    await _signInManager.PasswordSignInAsync(name, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неправильний логін чи (та) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginUser(User model)
        {
            if (ModelState.IsValid)
            {
                await _signInManager.SignInAsync(model, true);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var size = _context.Users.FirstOrDefault(u => u.UserName == name);

            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedDealer = await _context.Users.FindAsync(id);
            if (authorizedDealer == null)
            {
                return NotFound();
            }
            return View(authorizedDealer);
        }

        // POST: AuthorizedDealers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, Year")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && ValidateYear(user.Year))
            {
                try
                {
                    var user1 = await _userManager.FindByIdAsync(id);
                    user1.UserName = user.UserName;
                    user1.Year = user.Year;
                    await _userManager.UpdateAsync(user1);
                    await LoginUser(user1);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                var err = new IdentityError();
                err.Description = "Рік некоректний!";
                ModelState.AddModelError(string.Empty, err.Description);
            }
            return View(user);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool ValidateYear(int year)
        {
            return year >= 0 && year <= DateTime.Now.Year;
        }
    }
}
