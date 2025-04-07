using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using charolis.Models;
using charolis.Services;

namespace charolis.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;
        private readonly RegUserRepository _regRepo;

        public AccountController(IAuthService auth, RegUserRepository regRepo)
        {
            _auth    = auth;
            _regRepo = regRepo;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string name, string email, string password, string phone, string address)
        {
            // Створюємо нового користувача та хешуємо пароль через AuthService
            var user = new RegUser(name, email, "", phone, address);
            _auth.Register(user, password);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _auth.ValidateUser(email, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Невірний email або пароль");
                return View();
            }

            // Створення claims та підписання користувача через Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Main", "UI");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
