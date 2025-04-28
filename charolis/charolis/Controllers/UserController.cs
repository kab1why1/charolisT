using Microsoft.AspNetCore.Mvc;
using charolis.Entity;
using charolis.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace charolis.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: /User
        public IActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: /User/Details/5
        public IActionResult Details(int id)
        {
            var user = _service.GetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // GET: /User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            try
            {
                _service.Create(user);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(user);
            }
        }

        // GET: /User/Edit/5
        public IActionResult Edit(int id)
        {
            var user = _service.GetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /User/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            try
            {
                _service.Update(user);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(user);
            }
        }

        // GET: /User/Delete/5
        public IActionResult Delete(int id)
        {
            var user = _service.GetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
