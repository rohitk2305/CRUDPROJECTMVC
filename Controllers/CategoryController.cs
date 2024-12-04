using Microsoft.AspNetCore.Mvc;
using ProjectCRUDOperation.Models;
using ProjectCRUDOperation.Services;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Display Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // Add New Category
        public IActionResult Create()
        {
            return View();
        }

        // Post New Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            if (!ModelState.IsValid)
            {
                await _categoryService.AddCategoryAsync(category);
                TempData["SuccessMessage"] = "Category successfully added!";
                //return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Edit Category
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Post Edit Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(category);
                    TempData["SuccessMessage"] = "Category Updated successfully!";
                    //return RedirectToAction(nameof(Index));
                }
                catch
                {
                    if (await _categoryService.GetCategoryByIdAsync(category.CategoryId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(category);
        }

        // Delete Category
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Post Delete Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["SuccessMessage"] = "Category deleted successfully.";
            return View();
            //return RedirectToAction(nameof(Index));
        }
    }
}
