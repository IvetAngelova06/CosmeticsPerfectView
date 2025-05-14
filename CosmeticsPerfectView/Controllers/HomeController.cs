using System.Diagnostics;
using CosmeticsPerfectView.Models;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsPerfectView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
    }
}
