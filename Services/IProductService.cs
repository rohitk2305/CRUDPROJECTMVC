// IProductService.cs
using ProjectCRUDOperation.Models;
using ProjectCRUDOperation.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Services
{
    public interface IProductService
    {
        Task<ProductListViewModel> GetProductsAsync(int page, int pageSize);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Category>> GetCategoriesAsync();
    }
}
