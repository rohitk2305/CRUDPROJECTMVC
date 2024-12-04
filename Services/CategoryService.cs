using Microsoft.EntityFrameworkCore;
using ProjectCRUDOperation.Data;
using ProjectCRUDOperation.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all categories
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.category.ToListAsync();
        }

        // Get a category by id
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.category.FindAsync(id);
        }

        // Add a new category
        public async Task AddCategoryAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        // Update an existing category
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        // Delete a category
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.category.FindAsync(id);
            if (category != null)
            {
                _context.category.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
