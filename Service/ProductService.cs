using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<Product> AddProduct(Product product);
        Task<Product?> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public Task<Product?> GetProductById(int productId)
        {
            return _productRepository.GetProductById(productId);
        }

        public Task<Product> AddProduct(Product product)
        {
            return _productRepository.AddProduct(product);
        }

        public Task<Product?> UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }

        public Task<bool> DeleteProduct(int productId)
        {
            return _productRepository.DeleteProduct(productId);
        }
    }
}

