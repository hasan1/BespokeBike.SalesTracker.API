using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Service
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee?> GetEmployeeById(int employeeId);
        Task<int> AddEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(int employeeId);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public Task<Employee?> GetEmployeeById(int employeeId)
        {
            return _employeeRepository.GetEmployeeById(employeeId);
        }

        public Task<int> AddEmployee(Employee employee)
        {
            return _employeeRepository.AddEmployee(employee);
        }

        public Task<bool> UpdateEmployee(Employee employee)
        {
            return _employeeRepository.UpdateEmployee(employee);
        }

        public Task<bool> DeleteEmployee(int employeeId)
        {
            return _employeeRepository.DeleteEmployee(employeeId);
        }
    }
}

