using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.Repository;

namespace BespokeBike.SalesTracker.API.Service
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer?> GetCustomerById(int customerId);
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int customerId);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAllCustomers();
        }


        public async Task<Customer?> GetCustomerById(int customerId)
        {
            return await _customerRepository.GetCustomerById(customerId);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customerRepository.AddCustomer(customer);
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            return await _customerRepository.UpdateCustomer(customer);
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            return await _customerRepository.DeleteCustomer(customerId);
        }

    }
}
