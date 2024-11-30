using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectCRUDOperation.Data;
using ProjectCRUDOperation.Models;
using ProjectCRUDOperation.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display Product List with Pagination
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Total number of products
            var totalProducts = await _context.product.CountAsync();

            // Pagination logic
            var totalPages = (int)System.Math.Ceiling(totalProducts / (double)pageSize);

            // Get products for the current page
            var products = await _context.product
                .Include(p => p.Category)  // Include Category details
                .Skip((page - 1) * pageSize)  // Skip records for previous pages
                .Take(pageSize)  // Take records for the current page
                .ToListAsync();

            // Create ViewModel
            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        // Add New Product
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.category, "CategoryId", "CategoryName");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,CategoryId")] Product product)
        {
            if (!ModelState.IsValid)
            {
               
                try
                {
                    // Add the new product to the database
                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    // Store a success message in TempData
                    TempData["SuccessMessage"] = "Product successfully added!";
                    /*return RedirectToAction(nameof(Index)); */// Redirect back to the product list
                }
                catch (Exception)
                {
                    // If something goes wrong, show an error message
                    TempData["ErrorMessage"] = "There was an error adding the product. Please try again.";
                    //return RedirectToAction(nameof(Create)); // Stay on the Create page if there is an error
                }
            }

            // If model is invalid, return to the Create view with validation errors
            ViewData["CategoryId"] = new SelectList(_context.category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        // Edit Product
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Retrieve the product by ID
            var product = await _context.product
                .Include(p => p.Category)  // Include category details for the dropdown
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }


            ViewData["Category"] = new SelectList(await _context.category.ToListAsync(), "CategoryId", "CategoryName");
            return View(product);
        }

        // Post Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product Updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.product.Any(e => e.ProductId == product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                    TempData["ErrorMessage"] = "There was an error adding the product. Please try again.";
                }
                
            }

            ViewData["Category"] = new SelectList(await _context.category.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // Delete Product
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Post Delete Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.product.Remove(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product deleted successfully.";
            return View();
            //return RedirectToAction(nameof(Index));
            //return Ok();
        }
    }
}
