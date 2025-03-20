using Project_Group3_SWD.Extensions;
using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Project_Group3_SWD.ViewModels;
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
        private readonly GoogleAuthService _googleAuthService;

        public AuthController(IUserService userService, IEmailServices emailService, GoogleAuthService googleAuthService)
        {
            _userService = userService;
            _emailService = emailService;
            _googleAuthService = googleAuthService;
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
                try
                {
                    User user = await _userService.GetUserByEmailAndPassAsync(model.Email, model.Password);

                    if (user != null && user.Status == true)
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
                        ViewBag.ErrorMessage = "Invalid login attempt.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
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
                try
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
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("user");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult LoginWithGoogle()
        {
            try
            {
                var redirectUrl = Url.Action("GoogleResponse", "Auth");
                return _googleAuthService.ChallengeGoogleAuthentication(redirectUrl);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var user = await _googleAuthService.HandleGoogleResponseAsync();

                if (user == null)
                {
                    ViewBag.ErrorMessage = "Google login failed.";
                    return RedirectToAction("Login");
                }

                HttpContext.Session.SetObjectAsSession("user", user);
                return RedirectToUserDashboard(user.RoleId);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToAction("Login");
            }
        }

        private IActionResult RedirectToUserDashboard(int? roleId)
        {
            int finalRoleId = roleId ?? 3;

            return finalRoleId switch
            {
                1 => RedirectToAction("Index", "Brand", new { area = "Admin" }),
                2 => RedirectToAction("Index", "Order", new { area = "Saler" }),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}
