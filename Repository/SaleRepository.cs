using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAllSales();
        Task<Sale?> GetSaleById(int saleId);
        Task<int> AddSale(Sale sale);
        Task<bool> UpdateSale(Sale sale);
        Task<bool> DeleteSale(int saleId);
    }

    public class SaleRepository : ISaleRepository
    {
        private readonly IAppSettings _appSettings;

        public SaleRepository(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        // GET: Get all sales
        public async Task<IEnumerable<Sale>> GetAllSales()
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Sale>("SELECT * FROM [dbo].[Sale]");
            }
        }

        // GET: Get a sale by ID
        public async Task<Sale?> GetSaleById(int saleId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Sale>("SELECT * FROM [dbo].[Sale] WHERE SaleID = @SaleID", new { SaleID = saleId });
            }
        }

        // ADD: Insert a new sale
        public async Task<int> AddSale(Sale sale)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO [dbo].[Sale] (EmployeeID, CustomerID, SalesDate, TotalAmount, IsActive) 
                             VALUES (@EmployeeID, @CustomerID, @SalesDate, @TotalAmount, @IsActive);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return await connection.QuerySingleAsync<int>(query, sale);
            }
        }

        // UPDATE: Update an existing sale
        public async Task<bool> UpdateSale(Sale sale)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"UPDATE [dbo].[Sale] 
                             SET EmployeeID = @EmployeeID, CustomerID = @CustomerID, SalesDate = @SalesDate, 
                                 TotalAmount = @TotalAmount, IsActive = @IsActive 
                             WHERE SaleID = @SaleID";

                var rowsAffected = await connection.ExecuteAsync(query, sale);
                return rowsAffected > 0;
            }
        }

        // DELETE: Delete a sale by ID
        public async Task<bool> DeleteSale(int saleId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM [dbo].[Sale] WHERE SaleID = @SaleID";
                var rowsAffected = await connection.ExecuteAsync(query, new { SaleID = saleId });
                return rowsAffected > 0;
            }
        }
    }
}
