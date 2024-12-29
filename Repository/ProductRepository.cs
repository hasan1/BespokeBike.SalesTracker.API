using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<int> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
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
                return await connection.QueryAsync<Product>("SELECT [ProductID]      ,[Name]      ,[Manufacturer]      ,[Style]      ,[QuantityOnHand]      ,[IsActive]  FROM [dbo].[Product]");
            }
        }

        // GET: Get a product by ID
        public async Task<Product?> GetProductById(int productId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Product>("SELECT [ProductID]      ,[Name]      ,[Manufacturer]      ,[Style]      ,[QuantityOnHand]      ,[IsActive]  FROM [dbo].[Product] WHERE ProductID = @ProductID", new { ProductID = productId });
            }
        }

        // ADD: Insert a new product
        public async Task<int> AddProduct(Product product)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO [dbo].[Product] (Name, Manufacturer, Style, QuantityOnHand, IsActive) 
                             VALUES (@Name, @Manufacturer, @Style, @QuantityOnHand, @IsActive);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return await connection.QuerySingleAsync<int>(query, product);
            }
        }

        // UPDATE: Update an existing product
        public async Task<bool> UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"UPDATE [dbo].[Product] 
                             SET Name = @Name, Manufacturer = @Manufacturer, Style = @Style, 
                                 QuantityOnHand = @QuantityOnHand, IsActive = @IsActive 
                             WHERE ProductID = @ProductID";

                var rowsAffected = await connection.ExecuteAsync(query, product);
                return rowsAffected > 0;
            }
        }

        // DELETE: Delete a product by ID
        public async Task<bool> DeleteProduct(int productId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM [dbo].[Product] WHERE ProductID = @ProductID";
                var rowsAffected = await connection.ExecuteAsync(query, new { ProductID = productId });
                return rowsAffected > 0;
            }
        }
    }
}

