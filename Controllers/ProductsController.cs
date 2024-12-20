using Microsoft.AspNetCore.Mvc;
using ProyectoPrueba.Filters;
using ProyectoPrueba.Models;
using ProyectoPrueba.Services;

namespace ProyectoPrueba.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController()
        {
            var userId = HttpContext?.Session?.GetInt32("UserId");
            if (!userId.HasValue)
            {
                RedirectToAction("Login", "Account");
            }
            _productService = new ProductService(userId.GetValueOrDefault());
        }
        [AuthorizeUser]
        public IActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        [AuthorizeUser]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.Create(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [AuthorizeUser]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [AuthorizeUser]
        public ActionResult Delete(int id)
        {
            _productService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
