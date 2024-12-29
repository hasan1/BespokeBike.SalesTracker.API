using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee?> GetEmployeeById(int employeeId);
        Task<int> AddEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(int employeeId);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IAppSettings _appSettings;

        public EmployeeRepository(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        // GET: Get all employees
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Employee>("SELECT [EmployeeID]      ,[FirstName]      ,[LastName]      ,[Address]      ,[Phone]      ,[StartDate]      ,[TerminationDate]      ,[Manager]      ,[IsActive]  FROM [dbo].[Employee]\r\n");
            }
        }

        // GET: Get an employee by ID
        public async Task<Employee?> GetEmployeeById(int employeeId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Employee>("SELECT [EmployeeID]      ,[FirstName]      ,[LastName]      ,[Address]      ,[Phone]      ,[StartDate]      ,[TerminationDate]      ,[Manager]      ,[IsActive]  FROM [dbo].[Employee] WHERE EmployeeID = @EmployeeID", new { EmployeeID = employeeId });
            }
        }

        // ADD: Insert a new employee
        public async Task<int> AddEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO [dbo].[Employee] (FirstName, LastName, Address, Phone, StartDate, TerminationDate, Manager, IsActive) 
                             VALUES (@FirstName, @LastName, @Address, @Phone, @StartDate, @TerminationDate, @Manager, @IsActive);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return await connection.QuerySingleAsync<int>(query, employee);
            }
        }

        // UPDATE: Update an existing employee
        public async Task<bool> UpdateEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"UPDATE [dbo].[Employee] 
                             SET FirstName = @FirstName, LastName = @LastName, Address = @Address, 
                                 Phone = @Phone, StartDate = @StartDate, TerminationDate = @TerminationDate, 
                                 Manager = @Manager, IsActive = @IsActive 
                             WHERE EmployeeID = @EmployeeID";

                var rowsAffected = await connection.ExecuteAsync(query, employee);
                return rowsAffected > 0;
            }
        }

        // DELETE: Delete an employee by ID
        public async Task<bool> DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM [dbo].[Employee] WHERE EmployeeID = @EmployeeID";
                var rowsAffected = await connection.ExecuteAsync(query, new { EmployeeID = employeeId });
                return rowsAffected > 0;
            }
        }
    }
}

