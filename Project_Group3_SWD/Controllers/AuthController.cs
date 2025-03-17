using Project_Group3_SWD.Extensions;
using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Project_Group3_SWD.ViewModels;
using Project_Group3_SWD.Constants;
using Project_Group3_SWD.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;

namespace Project_Group3_SWD.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailServices _emailService;

        public AuthController(IUserService userService, IEmailServices emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.GetUserByEmailAndPassAsync(model.Email, model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetObjectAsSession("user", user);
                    if (user.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Brand", new { area = "Admin" });
                    }
                    else if (user.RoleId == 2)
                    {
                        return RedirectToAction("Index", "Order", new { area = "Saler" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Đăng nhập thất bại
                    ViewBag.ErrorMessage = "Invalid login attempt.";
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already in use.");

                    return View(model);
                }

                var user = new User
                {
                    Fullname = model.Fullname,
                    Email = model.Email,
                    Password = model.Password,
                    RoleId = 3,
                    Status = true,
                };

                var result = await _userService.CreateUserAsync(user);

                if (result)
                {
                    HttpContext.Session.SetObjectAsSession("user", user);

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = "Registration failed.";
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult LoginWithGoogle()