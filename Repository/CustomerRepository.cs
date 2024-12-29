using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer?> GetCustomerById(int customerId);
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int customerId);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IAppSettings _appSettings;

        public CustomerRepository(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        // GET: Get all customers
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Customer>("SELECT [CustomerID]      ,[FirstName]      ,[LastName]      ,[Address]      ,[Phone]      ,[StartDate]      ,[Email]      ,[IsActive]  FROM [dbo].[Customer]\r\n");
            }
        }

        // GET: Get a customer by ID
        public async Task<Customer?> GetCustomerById(int customerId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Customer>("SELECT [CustomerID]      ,[FirstName]      ,[LastName]      ,[Address]      ,[Phone]      ,[StartDate]      ,[Email]      ,[IsActive]  FROM [dbo].[Customer]\r\n WHERE CustomerID = @CustomerID", new { CustomerID = customerId });
            }
        }

        // ADD: Insert a new customer
        public async Task<Customer> AddCustomer(Customer customer)
        {
            customer.IsActive = true;
            int customerId  = 0;

            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO [dbo].[Customer] (FirstName, LastName, Address, Phone, StartDate, Email, IsActive) 
                             VALUES (@FirstName, @LastName, @Address, @Phone, @StartDate, @Email, @IsActive);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

                customerId = await connection.QuerySingleAsync<int>(query, customer);

            }

            return await GetCustomerById(customerId);
        }

        // UPDATE: Update an existing customer
        public async Task<Customer?> UpdateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"UPDATE [dbo].[Customer] 
                                 SET FirstName = @FirstName, LastName = @LastName, Address = @Address, 
                                     Phone = @Phone, StartDate = @StartDate, Email = @Email, IsActive = @IsActive 
                                 WHERE CustomerID = @CustomerID";

                var rowsAffected = await connection.ExecuteAsync(query, customer);
                return await GetCustomerById(customer.CustomerId);
            }
        }

        // DELETE: Delete a customer by ID
        public async Task<bool> DeleteCustomer(int customerId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM [dbo].[Customer] WHERE CustomerID = @CustomerID";
                var rowsAffected = await connection.ExecuteAsync(query, new { CustomerID = customerId });
                return rowsAffected > 0;
            }
        }
    }
}

