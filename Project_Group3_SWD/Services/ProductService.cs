using Project_Group3_SWD.Models;
using Project_Group3_SWD.Repositories;
using Project_Group3_SWD.ViewModels;
using Microsoft.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace Project_Group3_SWD.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product GetProductById(int productId)
        {
            return _productRepository.GetProductById(productId);
        }

		public Product GetProductByIdIncludeCategory(int id)
		{
			return _productRepository.GetProductByIdIncludeCategory(id);
		}

		public async Task<List<Product>> GetProductsAsync(int? brand, int? category, decimal? minPrice, decimal? maxPrice, string? searchQuery, string sortOrder)
        {
            return await _productRepository.GetProductsAsync(brand, category, minPrice, maxPrice, searchQuery, sortOrder);
        }

        public void reduceQuantity(List<Item> cart)
        {
            _productRepository.reduceQuantity(cart);
        }

        public async Task<List<Product>> top8NewestProduct()
        {
            return await _productRepository.top8NewestProduct();
        }
    }
}
