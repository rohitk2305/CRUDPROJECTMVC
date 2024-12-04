// ProductController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectCRUDOperation.Models;
using ProjectCRUDOperation.Services;
using ProjectCRUDOperation.ViewModels;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Display Product List with Pagination
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var model = await _productService.GetProductsAsync(page, pageSize);
            return View(model);
        }

        // Add New Product
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "CategoryName");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,CategoryId")] Product product)
        {
            if (!ModelState.IsValid)
            {
                bool success = await _productService.CreateProductAsync(product);
                if (success)
                {
                    TempData["SuccessMessage"] = "Product successfully added!";
                    //return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "There was an error adding the product. Please try again.";
                }
            }

            ViewData["CategoryId"] = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // Edit Product
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
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
                bool success = await _productService.UpdateProductAsync(product);
                if (success)
                {
                    TempData["SuccessMessage"] = "Product updated successfully!";
                   // return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "There was an error updating the product.";
                }
            }

            ViewData["CategoryId"] = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // Delete Product
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
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
            bool success = await _productService.DeleteProductAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Product deleted successfully.";
                return View();
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "There was an error deleting the product.";
                return View();
                //return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
