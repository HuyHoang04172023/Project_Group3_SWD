using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Project_Group3_SWD.Proxy
{
    public class GoogleAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IEmailServices _emailService;
        public const string GMAIL_REGISTER_EMAIL_SUBJECT = "Welcome to SHOPQUANAO!";

        public const string GMAIL_REGISTER_EMAIL_BODY = @"
                <html>
                <body>
                    <h2>Welcome to SHOPQUANAO!</h2>
                    <p>Your account has been successfully created via Google Sign-In. Here are your account details:</p>
                    <p><strong>Registered Email:</strong> {1}</p>
                    <p>If this wasn’t you, please contact our support team immediately.</p>
                    <br>
                    <p>Best regards,</p>
                    <p>SHOPQUANAO Team</p>
                    <p><a href='mailto:support@SHOPQUANAO.com'>support@SHOPQUANAO.com</a> | SHOPQUANAO</p>
                </body>
                </html>";

        public GoogleAuthService(IHttpContextAccessor httpContextAccessor, IUserService userService, IEmailServices emailService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _emailService = emailService;
        }

        public IActionResult ChallengeGoogleAuthentication(string redirectUrl)
        {
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<User> HandleGoogleResponseAsync()
        {
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return null;

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null)
                return null;

            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null || user.Status == true)
            {
                user = new User
                {
                    Fullname = name ?? "Google User",
                    Email = email,
                    Password = "",
                    RoleId = 3,
                    Status = true
                };

                string subject = GMAIL_REGISTER_EMAIL_SUBJECT;
                string body = string.Format(
                    GMAIL_REGISTER_EMAIL_BODY,
                    user.Fullname,
                    user.Email,
                    ""
                );

                await _userService.CreateUserAsync(user);
                await _emailService.SendEmail(email, subject, body);
            }

            return user;
        }
    }
}
