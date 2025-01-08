using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAllSales();
        Task<Sale?> GetSaleById(int saleId);
        Task<Sale> AddSale(Sale sale);
        Task<Sale?> UpdateSale(Sale sale);
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
                return await connection.QueryAsync<Sale>("GetAllSales", commandType: CommandType.StoredProcedure);
            }
        }

        // GET: Get a sale by ID
        public async Task<Sale?> GetSaleById(int saleId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Sale>("GetSaleById", new { SaleID = saleId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Sale> AddSale(Sale sale)
        {
            sale.IsActive = true;
            int saleId;

            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeID", sale.EmployeeId);
                parameters.Add("CustomerID", sale.CustomerId);
                parameters.Add("SalesDate", sale.SalesDate);
                parameters.Add("TotalAmount", sale.TotalAmount);
                parameters.Add("IsActive", sale.IsActive);
                parameters.Add("SaleID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("AddSale", parameters, commandType: CommandType.StoredProcedure);
                saleId = parameters.Get<int>("SaleID");
            }

            return await GetSaleById(saleId);
        }

        // UPDATE: Update an existing sale
        public async Task<Sale?> UpdateSale(Sale sale)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    sale.SaleId,
                    sale.EmployeeId,
                    sale.CustomerId,
                    sale.SalesDate,
                    sale.TotalAmount,
                    sale.IsActive
                };

                await connection.ExecuteAsync("UpdateSale", parameters, commandType: CommandType.StoredProcedure);
                return await GetSaleById(sale.SaleId);
            }
        }

        // DELETE: Delete a sale by ID
        public async Task<bool> DeleteSale(int saleId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var rowsAffected = await connection.ExecuteAsync("DeleteSale", new { SaleID = saleId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}
