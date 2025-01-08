using BespokeBike.SalesTracker.API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee?> GetEmployeeById(int employeeId);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee?> UpdateEmployee(Employee employee);
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
                return await connection.QueryAsync<Employee>("GetAllEmployees", commandType: CommandType.StoredProcedure);
            }
        }

        // GET: Get an employee by ID
        public async Task<Employee?> GetEmployeeById(int employeeId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Employee>("GetEmployeeById", new { EmployeeID = employeeId }, commandType: CommandType.StoredProcedure);
            }
        }

        // ADD: Insert a new employee
        public async Task<Employee> AddEmployee(Employee employee)
        {
            employee.IsActive = true;
            int employeeId;

            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("FirstName", employee.FirstName);
                parameters.Add("LastName", employee.LastName);
                parameters.Add("Address", employee.Address);
                parameters.Add("Phone", employee.Phone);
                parameters.Add("StartDate", employee.StartDate);
                parameters.Add("TerminationDate", employee.TerminationDate);
                parameters.Add("Manager", employee.Manager);
                parameters.Add("IsActive", employee.IsActive);
                parameters.Add("EmployeeID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("AddEmployee", parameters, commandType: CommandType.StoredProcedure);
                employeeId = parameters.Get<int>("EmployeeID");
            }

            return await GetEmployeeById(employeeId);
        }

        // UPDATE: Update an existing employee
        // UPDATE: Update an existing employee
        public async Task<Employee?> UpdateEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    employee.EmployeeId,
                    employee.FirstName,
                    employee.LastName,
                    employee.Address,
                    employee.Phone,
                    employee.StartDate,
                    employee.TerminationDate,
                    employee.Manager,
                    employee.IsActive
                };

                await connection.ExecuteAsync("UpdateEmployee", parameters, commandType: CommandType.StoredProcedure);
                return await GetEmployeeById(employee.EmployeeId);
            }
        }

        // DELETE: Delete an employee by ID
        public async Task<bool> DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(_appSettings.BespokeBikeDBconn))
            {
                await connection.OpenAsync();
                var rowsAffected = await connection.ExecuteAsync("DeleteEmployee", new { EmployeeID = employeeId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}

