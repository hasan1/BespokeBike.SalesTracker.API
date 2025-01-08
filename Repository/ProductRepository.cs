using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<Product> AddProduct(Product product);
        Task<Product?> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
        Task<bool> IsProductNameUnique(string productName, int productId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly IAppSettings _appSettings;

        public ProductRepository(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        // GET: Get all products
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Product>("GetAllProducts", commandType: CommandType.StoredProcedure);
            }
        }

        // GET: Get a product by ID
        public async Task<Product?> GetProductById(int productId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Product>("GetProductById", new { ProductID = productId }, commandType: CommandType.StoredProcedure);
            }
        }

        // ADD: Insert a new product
        public async Task<Product> AddProduct(Product product)
        {
            product.IsActive = true;
            int productId;

            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("Name", product.Name);
                parameters.Add("Manufacturer", product.Manufacturer);
                parameters.Add("Style", product.Style);
                parameters.Add("QuantityOnHand", product.QuantityOnHand);
                parameters.Add("IsActive", product.IsActive);
                parameters.Add("ProductId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("AddProduct", parameters, commandType: CommandType.StoredProcedure);
                productId = parameters.Get<int>("ProductId");
            }

            return await GetProductById(productId);
        }

        // UPDATE: Update an existing product
        public async Task<Product?> UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    product.ProductId,
                    product.Name,
                    product.Manufacturer,
                    product.Style,
                    product.QuantityOnHand,
                    product.IsActive
                };

                await connection.ExecuteAsync("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
                return await GetProductById(product.ProductId);
            }
        }


        // DELETE: Delete a product by ID
        public async Task<bool> DeleteProduct(int productId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var rowsAffected = await connection.ExecuteAsync("DeleteProduct", new { ProductID = productId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> IsProductNameUnique(string productName, int productId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleAsync<int>("CheckProductNameUnique",
                    new { Name = productName , ProductId = productId }, commandType: CommandType.StoredProcedure);
                return result == 1;
            }
        }
    }
}

