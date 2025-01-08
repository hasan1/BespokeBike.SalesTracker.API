using AutoMapper;
using BespokeBike.SalesTracker.API.Extensions;
using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.ModelDto;
using BespokeBike.SalesTracker.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace BespokeBike.SalesTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, IMapper mapper, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CustomerGetDto>>>> GetAllCustomers()
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var customers = await _customerService.GetAllCustomers();
                var customerDtos = _mapper.Map<IEnumerable<CustomerGetDto>>(customers);
                return this.ToApiResponse(customerDtos, "Customers retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving customers. Correlation ID: {correlationId}", correlationId);
                return this.ToApiResponse<IEnumerable<CustomerGetDto>>(ex.Message, 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerGetDto>>> GetCustomerById(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                if (customer == null)
                {
                    return this.ToApiResponse<CustomerGetDto>("Customer not found", 404);
                }               
                var customerDto = _mapper.Map<CustomerGetDto>(customer);
                return this.ToApiResponse(customerDto, "Customer retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving customer with ID {id}. Correlation ID: {correlationId}", correlationId);
                return this.ToApiResponse<CustomerGetDto>(ex.Message, 500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Customer>>> AddCustomer([FromBody] CustomerCreateDto customerCreateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var customer = _mapper.Map<Customer>(customerCreateDto);
                var result = await _customerService.AddCustomer(customer);
                return this.ToApiResponse(result, "Customer created successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding a new customer. Correlation ID: {correlationId}. Input: {customerCreateDto}");
                return this.ToApiResponse<Customer>(ex.Message, 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Customer>>> UpdateCustomer(int id, [FromBody] CustomerUpdateDto customerUpdateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                if (id != customerUpdateDto.CustomerId)
                {
                    return this.ToApiResponse<Customer>("Customer ID mismatch", 400);
                }

                var customer = _mapper.Map<Customer>(customerUpdateDto);
                var result = await _customerService.UpdateCustomer(customer);
                return this.ToApiResponse(result, "Customer updated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating customer with ID {id}. Correlation ID: {correlationId}. Input: {customerUpdateDto}");
                return this.ToApiResponse<Customer>(ex.Message, 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var result = await _customerService.DeleteCustomer(id);
                if (!result)
                {
                    return this.ToApiResponse<bool>("Customer not found", 404);
                }
                return this.ToApiResponse(true, "Customer deleted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting customer with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<bool>(ex.Message, 500);
            }
        }
    }
}