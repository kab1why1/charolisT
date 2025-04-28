using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using charolis.Entity;
using charolis.Services.Interfaces;

namespace YourNamespace.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;  // <-- ІНТЕРФЕЙС

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET: /Product
        public IActionResult Index()
        {
            var products = _service.GetAll();
            return View(products);
        }

        // GET: /Product/Details/5
        public IActionResult Details(int id)
        {
            var product = _service.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                _service.Create(product);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(product);
            }
        }

        // GET: /Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _service.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: /Product/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                _service.Update(product);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(product);
            }
        }

        // GET: /Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _service.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
