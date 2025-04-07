    using System;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    namespace charolis.Models;

    public class UIController : Controller
    {   
        private readonly AdminRepository _adminRepository;
        private readonly RegUserRepository _regUserRepository;
        private readonly GuessRepository _guessRepository;
        private readonly OrdersRepository _ordersRepository;
        private readonly ProductRepository _productRepository;

        public UIController() {
            _adminRepository = new AdminRepository();
            _regUserRepository = new RegUserRepository();
            _guessRepository = new GuessRepository();
            _ordersRepository = new OrdersRepository();
            _productRepository = new ProductRepository();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Main");
        }

        public IActionResult Main()
        {
            return View();
        }


        // admins crud
        public IActionResult Admins() => View(_adminRepository.GetSorted());
        public IActionResult CreateAdmin() => View();

        [HttpPost]
        public IActionResult CreateAdmin(Admin admin)
        {
            _adminRepository.Add(admin);
            return RedirectToAction("Admins");
        }

        public IActionResult EditAdmin(int id) => View(_adminRepository.GetById(id));

        [HttpPost]
        public IActionResult EditAdmin(Admin admin) 
        {
            var existingAdmin = _adminRepository.GetById(admin.Id);
            if (existingAdmin != null)
            {
                _adminRepository.Remove(existingAdmin);
            } 
            _adminRepository.Add(admin);
            return RedirectToAction("Admins");
        }

        public IActionResult DeleteAdmin(int id)
        {
            var admin = _adminRepository.GetById(id);
            if (admin != null)
            {
                _adminRepository.Remove(admin);
            }
            return RedirectToAction("Admins");
        }

        // RegUsers CRUD

        public IActionResult RegUsers() => View(_regUserRepository.GetSorted());
        public IActionResult CreateRegUser() => View();

        [HttpPost]
        public IActionResult CreateRegUser(RegUser regUser)
        {
            _regUserRepository.Add(regUser);
            return RedirectToAction("RegUsers");
        }

        public IActionResult EditRegUser(int id) => View(_regUserRepository.GetById(id));
        [HttpPost]
        public IActionResult EditRegUser(RegUser regUser)
        {
            var existingRegUser = _regUserRepository.GetById(regUser.Id);
            if(existingRegUser != null)
            {
                _regUserRepository.Remove(existingRegUser);
            }
            _regUserRepository.Add(regUser);
            return RedirectToAction("RegUsers");
        }

        public IActionResult DeleteRegUser(int id)
        {
            var regUser = _regUserRepository.GetById(id);
            if(regUser != null)
            {
                _regUserRepository.Remove(regUser);
            }
            return RedirectToAction("RegUsers");
        }

        // Products CRUD
        public IActionResult Products() => View(_productRepository.GetSorted());
        public IActionResult CreateProduct() => View();

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            _productRepository.Add(product);
            return RedirectToAction("Products");
        }

        public IActionResult EditProduct(int id) => View(_productRepository.GetById(id));

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            var existingProduct = _productRepository.GetById(product.Id);
            if(existingProduct != null)
            {
                _productRepository.Remove(existingProduct);
            }
            _productRepository.Add(product);
            return RedirectToAction("Products");
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                _productRepository.Remove(product);
            }
            return RedirectToAction("Products");
        }

        // Orders CRUD

            // Відображення списку всіх замовлень
            public IActionResult Orders() => View(_ordersRepository.GetSorted());

            // GET: Створення замовлення
            public IActionResult CreateOrder()
            {
                // Передаємо існуючих користувачів та продукти у View через ViewBag
                ViewBag.RegUsers = _regUserRepository.GetSorted();
                ViewBag.Products = _productRepository.GetSorted();
                return View();
            }

            // POST: Створення замовлення
            [HttpPost]
            public IActionResult CreateOrder(Order order, int regUserId, List<int> productIds)
            {
                // Встановлюємо існуючого користувача
                order.Customer = _regUserRepository.GetById(regUserId);
                // Формуємо список продуктів за вибраними id
                order.Products = _productRepository.GetAll().Where(p => productIds.Contains(p.Id)).ToList();
                // Перераховуємо загальну суму та кількість продуктів
                order.TotalPrice = order.Products.Sum(p => p.Price);
                order.ProductCount = order.Products.Count;
                _ordersRepository.Add(order);
                return RedirectToAction("Orders");
            }

            // GET: Редагування замовлення
            public IActionResult EditOrder(int id)
            {
                var order = _ordersRepository.GetById(id);
                ViewBag.RegUsers = _regUserRepository.GetSorted();
                ViewBag.Products = _productRepository.GetSorted();
                return View(order);
            }

            // POST: Редагування замовлення
            [HttpPost]
            public IActionResult EditOrder(Order order, int regUserId, List<int> productIds)
            {
                order.Customer = _regUserRepository.GetById(regUserId);
                order.Products = _productRepository.GetAll().Where(p => productIds.Contains(p.Id)).ToList();
                order.TotalPrice = order.Products.Sum(p => p.Price);
                order.ProductCount = order.Products.Count;
                // Оновлюємо замовлення: видаляємо старе та додаємо нове (або оновлюємо властивості)
                var existingOrder = _ordersRepository.GetById(order.Id);
                if(existingOrder != null)
                {
                    _ordersRepository.Remove(existingOrder);
                }
                _ordersRepository.Add(order);
                return RedirectToAction("Orders");
            }

            // Видалення замовлення
            public IActionResult DeleteOrder(int id)
            {
                var order = _ordersRepository.GetById(id);
                if(order != null)
                {
                    _ordersRepository.Remove(order);
                }
                return RedirectToAction("Orders");
            }
    }
