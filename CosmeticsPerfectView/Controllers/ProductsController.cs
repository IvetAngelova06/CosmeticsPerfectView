using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmeticsPerfectView.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CosmeticsPerfectView.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IActionResult AddToCart(int id)
        {
            List<int> cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
            if (!cart.Contains(id))
                cart.Add(id);
            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Cart", "Home");
        }

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Discounted()
        //{
        //    var discountedProducts = _context.Products
        //        .Where(p => p.PromoPercent > 0)
        //        .ToList();

        //    return View(discountedProducts);
        //}

        // GET: Products
        public async Task<IActionResult> Index(string categ)
        {
            //if (categ == null)
            //{
            //    var applicationDbContext = _context.Products
            //    .Include(p => p.Categories)
            //    .Include(p => p.ProductTypes);
            //    return View(await applicationDbContext.ToListAsync());
            //}
            //else
            //{
            //    var applicationDbContext = _context.Products
            //    .Include(p => p.Categories)
            //    .Include(p => p.ProductTypes)
            //    .Where(x => x.Categories.Name == categ);
            //    return View(await applicationDbContext.ToListAsync());
            //}

            IQueryable<Product> applicationDbContext = _context.Products
         .Include(p => p.Categories)
         .Include(p => p.ProductTypes);

            // Ако няма избрана категория (клик на "Промоции"), показваме само намалените продукти
            if (string.IsNullOrEmpty(categ))
            {
                applicationDbContext = applicationDbContext.Where(p => p.PromoPercent > 0);
            }
            else
            {
                // Ако има категория, показваме всички продукти в тази категория (независимо дали са намалени)
                applicationDbContext = applicationDbContext.Where(x => x.Categories.Name == categ);
            }

            return View(await applicationDbContext.ToListAsync());

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.ProductTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CategoriesId,ProductTypeId,URLimage,Price,PromoPercent,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.DateRegister = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoriesId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoriesId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoriesId,ProductTypeId,URLimage,Price,PromoPercent,Description,DateRegister")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoriesId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.ProductTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
