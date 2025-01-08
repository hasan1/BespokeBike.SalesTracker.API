using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.Repository;

namespace BespokeBike.SalesTracker.API.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<Product> AddProduct(Product product);
        Task<Product?> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
        Task<bool> IsProductNameUnique(string productName, int productId);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }

        public async Task<Product> AddProduct(Product product)
        {
            return await _productRepository.AddProduct(product);
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateProduct(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }

        public async Task<bool> IsProductNameUnique(string productName, int productId)
        {
            return await _productRepository.IsProductNameUnique(productName, productId);
        }
    }
}

