using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using charolis.Services.Interfaces;
using charolis.Entity;
using charolis.Models.ViewModels;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Знаходимо користувача за логіном
            var user = _userService.GetAll().FirstOrDefault(u => u.Username == vm.Username);

            // Перевіряємо, що в нього є пароль (хеш) і що введений пароль не null
            if (user == null 
                || string.IsNullOrWhiteSpace(user.Password) 
                || string.IsNullOrWhiteSpace(vm.Password) 
                || !BCrypt.Net.BCrypt.Verify(vm.Password, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Неправильний логін або пароль.");
                return View(vm);
            }

            // Якщо все гаразд — формуємо Claims і логінимо
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };
            var identity   = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal  = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true }
            );

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (_userService.GetAll().Any(u => u.Username == vm.Username))
            {
                ModelState.AddModelError(nameof(vm.Username), "Логін уже зайнятий.");
                return View(vm);
            }

            var newUser = new User
            {
                Username = vm.Username,
                // Хешуємо пароль одразу
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),
                Email    = vm.Email,
                Phone    = vm.Phone,
                Address  = vm.Address,
                Role     = "User"
            };

            _userService.Create(newUser);
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
