using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using charolis.Models;

namespace charolis.Controllers
{
    [Authorize] // Всі дії потребують аутентифікації, окрім позначених [AllowAnonymous]
    public class UIController : Controller
    {
        private readonly AdminRepository   _adminRepository;
        private readonly RegUserRepository _regUserRepository;
        private readonly GuessRepository   _guessRepository;
        private readonly OrdersRepository  _ordersRepository;
        private readonly ProductRepository _productRepository;

        public UIController(
            AdminRepository   adminRepo,
            RegUserRepository regRepo,
            GuessRepository   guessRepo,
            OrdersRepository  ordersRepo,
            ProductRepository productRepo)
        {
            _adminRepository   = adminRepo;
            _regUserRepository = regRepo;
            _guessRepository   = guessRepo;
            _ordersRepository  = ordersRepo;
            _productRepository = productRepo;
        }

        [AllowAnonymous]
        public IActionResult Main() => View();

        // ----- Адмінський функціонал -----

        [Authorize(Roles = "admin")]
        public IActionResult Admins() => View(_adminRepository.GetSorted());

        [Authorize(Roles = "admin")]
        public IActionResult CreateAdmin() => View();

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult CreateAdmin(Admin admin)
        {
            _adminRepository.Add(admin);
            return RedirectToAction("Admins");
        }

        [Authorize(Roles = "admin")]
        public IActionResult EditAdmin(int id) => View(_adminRepository.GetById(id)!);

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult EditAdmin(Admin admin)
        {
            var existing = _adminRepository.GetById(admin.Id);
            if (existing != null) _adminRepository.Remove(existing);
            _adminRepository.Add(admin);
            return RedirectToAction("Admins");
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteAdmin(int id)
        {
            var admin = _adminRepository.GetById(id);
            if (admin != null) _adminRepository.Remove(admin);
            return RedirectToAction("Admins");
        }

        // ----- Зареєстровані користувачі (адмін тільки) -----

        [Authorize(Roles = "admin")]
        public IActionResult RegUsers() => View(_regUserRepository.GetSorted());

        [Authorize(Roles = "admin")]
        public IActionResult CreateRegUser() => View();

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult CreateRegUser(RegUser regUser)
        {
            _regUserRepository.Add(regUser);
            return RedirectToAction("RegUsers");
        }

        [Authorize(Roles = "admin")]
        public IActionResult EditRegUser(int id) => View(_regUserRepository.GetById(id)!);

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult EditRegUser(RegUser regUser)
        {
            var existing = _regUserRepository.GetById(regUser.Id);
            if (existing != null) _regUserRepository.Remove(existing);
            _regUserRepository.Add(regUser);
            return RedirectToAction("RegUsers");
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteRegUser(int id)
        {
            var user = _regUserRepository.GetById(id);
            if (user != null) _regUserRepository.Remove(user);
            return RedirectToAction("RegUsers");
        }

        // ----- Продукти (всі аутентифіковані) -----

        public IActionResult Products() => View(_productRepository.GetSorted());

        [Authorize(Roles = "admin")]
        public IActionResult CreateProduct() => View();

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult CreateProduct(Product product)
        {
            _productRepository.Add(product);
            return RedirectToAction("Products");
        }

        [Authorize(Roles = "admin")]
        public IActionResult EditProduct(int id) => View(_productRepository.GetById(id)!);

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult EditProduct(Product product)
        {
            var existing = _productRepository.GetById(product.Id);
            if (existing != null) _productRepository.Remove(existing);
            _productRepository.Add(product);
            return RedirectToAction("Products");
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteProduct(int id)
        {
            var prod = _productRepository.GetById(id);
            if (prod != null) _productRepository.Remove(prod);
            return RedirectToAction("Products");
        }

        // ----- Замовлення (Registered User та admin) -----

        [Authorize(Roles = "Registered User,admin")]
        public IActionResult Orders() => View(_ordersRepository.GetSorted());

        [Authorize(Roles = "Registered User,admin")]
        public IActionResult CreateOrder()
        {
            ViewBag.RegUsers = _regUserRepository.GetSorted();
            ViewBag.Products = _productRepository.GetSorted();
            return View();
        }

        [HttpPost, Authorize(Roles = "Registered User,admin")]
        public IActionResult CreateOrder(Order order, int regUserId, List<int> productIds)
        {
            order.Customer     = _regUserRepository.GetById(regUserId)!;
            order.Products     = _productRepository.GetAll().Where(p => productIds.Contains(p.Id)).ToList();
            order.TotalPrice   = order.Products.Sum(p => p.Price);
            order.ProductCount = order.Products.Count;
            _ordersRepository.Add(order);
            return RedirectToAction("Orders");
        }

        [Authorize(Roles = "Registered User,admin")]
        public IActionResult EditOrder(int id)
        {
            ViewBag.RegUsers = _regUserRepository.GetSorted();
            ViewBag.Products = _productRepository.GetSorted();
            return View(_ordersRepository.GetById(id)!);
        }

        [HttpPost, Authorize(Roles = "Registered User,admin")]
        public IActionResult EditOrder(Order order, int regUserId, List<int> productIds)
        {
            order.Customer     = _regUserRepository.GetById(regUserId)!;
            order.Products     = _productRepository.GetAll().Where(p => productIds.Contains(p.Id)).ToList();
            order.TotalPrice   = order.Products.Sum(p => p.Price);
            order.ProductCount = order.Products.Count;

            var existing = _ordersRepository.GetById(order.Id);
            if (existing != null) _ordersRepository.Remove(existing);
            _ordersRepository.Add(order);
            return RedirectToAction("Orders");
        }

        [Authorize(Roles = "Registered User,admin")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _ordersRepository.GetById(id);
            if (order != null) _ordersRepository.Remove(order);
            return RedirectToAction("Orders");
        }
    }
}
