using ProjectCRUDOperation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
