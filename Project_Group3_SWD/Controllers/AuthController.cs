using A_LIÊM_SHOP.Extensions;
using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Services;
using A_LIÊM_SHOP.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace A_LIÊM_SHOP.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
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
                    // Lưu đối tượng user vào session
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
						// Đăng nhập thành công
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
				// Kiểm tra email có tồn tại chưa
				var existingUser = await _userService.GetUserByEmailAsync(model.Email);
				if (existingUser != null)
				{
					// Thêm lỗi vào ModelState để hiển thị thông báo lỗi
					ModelState.AddModelError("Email", "Email already in use.");

					// Trả lại view và giữ lại dữ liệu đã nhập
					return View(model);
				}

				// Tạo người dùng mới
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
					// Lưu đối tượng user vào session
					HttpContext.Session.SetObjectAsSession("user", user);

					// Đăng nhập thành công
					return RedirectToAction("Index", "Home");
				}

				ViewBag.ErrorMessage = "Registration failed.";
			}

			return View(model);
		}

		public IActionResult Logout()
        {
            // Xóa thông tin người dùng khỏi session
            HttpContext.Session.Remove("user");

            // Chuyển hướng về trang login
            return RedirectToAction("Login", "Auth");
        }
    }
}
