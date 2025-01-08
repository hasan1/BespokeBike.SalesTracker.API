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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeGetDto>>>> GetAllEmployees()
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var employees = await _employeeService.GetAllEmployees();
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeGetDto>>(employees);
                return this.ToApiResponse(employeeDtos, "Employees retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving employees. Correlation ID: {correlationId}");
                return this.ToApiResponse<IEnumerable<EmployeeGetDto>>(ex.Message, 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeGetDto>>> GetEmployeeById(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var employee = await _employeeService.GetEmployeeById(id);
                if (employee == null)
                {
                    return this.ToApiResponse<EmployeeGetDto>("Employee not found", 404);
                }
                var employeeDto = _mapper.Map<EmployeeGetDto>(employee);
                return this.ToApiResponse(employeeDto, "Employee retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving employee with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<EmployeeGetDto>(ex.Message, 500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Employee>>> AddEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var employee = _mapper.Map<Employee>(employeeCreateDto);
                var result =  await _employeeService.AddEmployee(employee);
                return this.ToApiResponse(result, "Employee created successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding a new employee. Correlation ID: {correlationId}. Input: {employeeCreateDto}");
                return this.ToApiResponse<Employee>(ex.Message, 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto employeeUpdateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                if (id != employeeUpdateDto.EmployeeId)
                {
                    return this.ToApiResponse<Employee>("Employee ID mismatch", 400);
                }

                var employee = _mapper.Map<Employee>(employeeUpdateDto);
                var result = await _employeeService.UpdateEmployee(employee);
                return this.ToApiResponse(result, "Employee updated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating employee with ID {id}. Correlation ID: {correlationId}. Input: {employeeUpdateDto}");
                return this.ToApiResponse<Employee>(ex.Message, 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEmployee(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var result = await _employeeService.DeleteEmployee(id);
                if (!result)
                {
                    return this.ToApiResponse<bool>("Employee not found", 404);
                }
                return this.ToApiResponse(true, "Employee deleted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting employee with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<bool>(ex.Message, 500);
            }
        }
    }
}

