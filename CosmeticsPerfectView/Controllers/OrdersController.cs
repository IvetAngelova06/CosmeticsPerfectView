using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmeticsPerfectView.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CosmeticsPerfectView.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var borsaDbContext = _context.Orders
                                    .Include(o => o.Users)
                                    .Include(o => o.Products);
                return View(await borsaDbContext.ToListAsync());
            }
            else
            {
                var borsaDbContext = _context.Orders
                                    .Include(o => o.Users)
                                    .Include(o => o.Products)
                                    .Where(x => x.UserId == _userManager.GetUserId(User));
                return View(await borsaDbContext.ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var cartIds = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

            if (!cartIds.Any())
            {
                TempData["Message"] = "Количката е празна.";
                return RedirectToAction("Cart", "Home");
            }

            var products = _context.Products
                .Where(p => cartIds.Contains(p.Id))
                .ToList();

            // Тук можеш да добавиш логика за записване на поръчката в база

            HttpContext.Session.Remove("Cart"); // Изчистваме количката

            return View("Confirmation", products); // показваме поръчката
        
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductsId,OrderQuantity,Description")] Order order)
        {
            if (ModelState.IsValid)
            {
               order.UserId=_userManager.GetUserId(User);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", order.ProductsId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null)
            {  
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", order.ProductsId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductsId,OrderQuantity,Description")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", order.ProductsId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        public async Task<IActionResult> ConfirmOrder()
        {
            var cartIds = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

            if (!cartIds.Any())
            {
                TempData["Message"] = "Количката е празна.";
                return RedirectToAction("Cart", "Home");
            }

            var userId = _userManager.GetUserId(User);

            foreach (var productId in cartIds)
            {
                var order = new Order
                {
                    ProductsId = productId,
                    UserId = userId,
                    OrderQuantity = 1,
                    Description = "Поръчано от количката"
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }
    }
}
