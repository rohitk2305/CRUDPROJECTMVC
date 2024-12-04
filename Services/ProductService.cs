// ProductService.cs
using Microsoft.EntityFrameworkCore;
using ProjectCRUDOperation.Data;
using ProjectCRUDOperation.Models;
using ProjectCRUDOperation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCRUDOperation.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductListViewModel> GetProductsAsync(int page, int pageSize)
        {
            var totalProducts = await _context.product.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalProducts / (double)pageSize);

            var products = await _context.product
                .Include(p => p.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.product.FindAsync(id);
                if (product != null)
                {
                    _context.product.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.category.ToListAsync();
        }
    }
}
