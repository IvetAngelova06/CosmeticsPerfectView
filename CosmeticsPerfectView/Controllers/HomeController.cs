using System.Diagnostics;
using CosmeticsPerfectView.Data;
using CosmeticsPerfectView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticsPerfectView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Cart()
        {
            var cartIds = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

            var productsInCart = _context.Products
                .Where(p => cartIds.Contains(p.Id))
                .ToList();

            return View(productsInCart);
        }
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

            if (cart.Contains(id))
                cart.Remove(id);

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Cart");
        }
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Cart");
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Begin()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dior()
        {
            return View();
        }
        public IActionResult YSL()
        {
            return View();
        }
        public IActionResult Avene()
        {
            return View();
        }
        public IActionResult SDJ()
        {
            return View();
        }
        public IActionResult Kerastase()
        {
            return View();
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddToCart(int id)
        {
            List<int> cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

            if (!cart.Contains(id))
                cart.Add(id);

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Cart");
        }

    }
}
