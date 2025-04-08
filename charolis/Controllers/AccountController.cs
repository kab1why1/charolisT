using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using charolis.Models;
using charolis.Services;

namespace charolis.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;
        private readonly RegUserRepository _regRepo;
        private readonly AdminRepository _adminRepo;

        public AccountController(IAuthService auth, RegUserRepository regRepo, AdminRepository adminRepo)
        {
            _auth = auth;
            _regRepo = regRepo;
            _adminRepo = adminRepo;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(string name, string email, string password, string phone, string address)
        {
            var user = new RegUser(name, email, "", phone, address);
            _auth.Register(user, password);
            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _auth.ValidateUser(email, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Невірний email або пароль");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Main", "UI");
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied() => View();

        // GET: /Account/SwitchToAdmin
        // Демонстраційна дія, яка перемикає поточного користувача в режим адміністратора.
        [HttpGet]
        public async Task<IActionResult> SwitchToAdmin()
        {
            // Отримуємо першого адміністратора з репозиторію
            var admin = _adminRepo.GetAll().FirstOrDefault();
            if (admin == null)
            {
                // Якщо адміністратора ще немає – переходимо до сторінки створення нового адміністратора.
                return RedirectToAction("CreateAdmin", "UI");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Role, admin.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Оновлюємо поточну автентифікацію: виходимо і входимо заново з новими claims.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Main", "UI");
        }
    }
}
