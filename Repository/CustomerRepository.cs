using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

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
                return await connection.QueryAsync<Customer>("GetAllCustomers", commandType: CommandType.StoredProcedure);
            }
        }

        // GET: Get a customer by ID
        public async Task<Customer?> GetCustomerById(int customerId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Customer>("GetCustomerById", new { CustomerID = customerId }, commandType: CommandType.StoredProcedure);
            }
        }

        // ADD: Insert a new customer
        public async Task<Customer> AddCustomer(Customer customer)
        {
            customer.IsActive = true;
            int customerId;

            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("FirstName", customer.FirstName);
                parameters.Add("LastName", customer.LastName);
                parameters.Add("Address", customer.Address);
                parameters.Add("Phone", customer.Phone);
                parameters.Add("StartDate", customer.StartDate);
                parameters.Add("Email", customer.Email);
                parameters.Add("IsActive", customer.IsActive);
                parameters.Add("CustomerID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("AddCustomer", parameters, commandType: CommandType.StoredProcedure);
                customerId = parameters.Get<int>("CustomerID");
            }


            return await GetCustomerById(customerId);
        }

        // UPDATE: Update an existing customer
        public async Task<Customer?> UpdateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    customer.CustomerId,
                    customer.FirstName,
                    customer.LastName,
                    customer.Address,
                    customer.Phone,
                    customer.StartDate,
                    customer.Email,
                    customer.IsActive
                };

                await connection.ExecuteAsync("UpdateCustomer", parameters, commandType: CommandType.StoredProcedure);
                return await GetCustomerById(customer.CustomerId);
            }
        }

        // DELETE: Delete a customer by ID
        public async Task<bool> DeleteCustomer(int customerId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var rowsAffected = await connection.ExecuteAsync("DeleteCustomer", new { CustomerID = customerId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}

