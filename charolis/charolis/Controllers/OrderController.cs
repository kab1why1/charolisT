// Controllers/OrderController.cs

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using charolis.Services.Interfaces;
using System.Linq;

namespace YourNamespace.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService   _orderService;
        private readonly IUserService    _userService;
        private readonly IProductService _productService;

        public OrderController(
            IOrderService orderService,
            IUserService  userService,
            IProductService productService)
        {
            _orderService   = orderService;
            _userService    = userService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var orders = _orderService.GetAll();
            return View(orders);
        }

        public IActionResult Create()
        {
            PopulateViewBags();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(int userId, List<int> productIds, List<int> quantities)
        {
            // UI-валидація
            if (userId == 0)
                ModelState.AddModelError("userId", "Оберіть користувача.");
            if (productIds == null || !productIds.Any())
                ModelState.AddModelError("productIds", "Виберіть хоча б один товар.");

            if (!ModelState.IsValid)
            {
                PopulateViewBags(userId);
                return View();
            }

            try
            {
                var dict = productIds
                    .Select((id, idx) => new { id, qty = (idx < quantities.Count && quantities[idx] > 0) ? quantities[idx] : 1 })
                    .ToDictionary(x => x.id, x => x.qty);

                _orderService.Create(userId, dict);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateViewBags(userId);
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null) return NotFound();

            PopulateViewBags(order.UserId);
            return View(order);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int userId, List<int> productIds, List<int> quantities)
        {
            if (userId == 0)
                ModelState.AddModelError("userId", "Оберіть користувача.");
            if (productIds == null || !productIds.Any())
                ModelState.AddModelError("productIds", "Виберіть хоча б один товар.");

            if (!ModelState.IsValid)
            {
                PopulateViewBags(userId);
                return View(_orderService.GetById(id));
            }

            try
            {
                var order = _orderService.GetById(id)!;
                order.UserId = userId;
                var dict = productIds
                    .Select((pid, idx) => new { pid, qty = (idx < quantities.Count && quantities[idx] > 0) ? quantities[idx] : 1 })
                    .ToDictionary(x => x.pid, x => x.qty);

                _orderService.Update(order, dict);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateViewBags(userId);
                return View(_orderService.GetById(id));
            }
        }

        public IActionResult Delete(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _orderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateViewBags(int? selectedUserId = null)
        {
            ViewBag.Users = _userService.GetAll()
                .Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text  = u.Username,
                    Selected = selectedUserId.HasValue && u.Id == selectedUserId.Value
                })
                .ToList();

            ViewBag.Products = _productService.GetAll()
                .Where(p => p.IsActive)
                .Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text  = p.Name
                })
                .ToList();
        }
    }
}
