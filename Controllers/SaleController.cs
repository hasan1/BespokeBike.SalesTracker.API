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
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ISaleService saleService, IMapper mapper, ILogger<SaleController> logger)
        {
            _saleService = saleService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SaleGetDto>>>> GetAllSales()
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var sales = await _saleService.GetAllSales();
                var saleDtos = _mapper.Map<IEnumerable<SaleGetDto>>(sales);
                return this.ToApiResponse(saleDtos, "Sales retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving sales. Correlation ID: {correlationId}");
                return this.ToApiResponse<IEnumerable<SaleGetDto>>(ex.Message, 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SaleGetDto>>> GetSaleById(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var sale = await _saleService.GetSaleById(id);
                if (sale == null)
                {
                    return this.ToApiResponse<SaleGetDto>("Sale not found", 404);
                }
                var saleDto = _mapper.Map<SaleGetDto>(sale);
                return this.ToApiResponse(saleDto, "Sale retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving sale with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<SaleGetDto>(ex.Message, 500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SaleCreateDto>>> AddSale([FromBody] SaleCreateDto saleCreateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var sale = _mapper.Map<Sale>(saleCreateDto);
                await _saleService.AddSale(sale);
                return CreatedAtAction(nameof(GetSaleById), new { id = sale.SaleId }, this.ToApiResponse(saleCreateDto, "Sale created successfully", 201).Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding a new sale. Correlation ID: {correlationId}. Input: {saleCreateDto}");
                return this.ToApiResponse<SaleCreateDto>(ex.Message, 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SaleUpdateDto>>> UpdateSale(int id, [FromBody] SaleUpdateDto saleUpdateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                if (id != saleUpdateDto.SaleId)
                {
                    return this.ToApiResponse<SaleUpdateDto>("Sale ID mismatch", 400);
                }

                var sale = _mapper.Map<Sale>(saleUpdateDto);
                await _saleService.UpdateSale(sale);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating sale with ID {id}. Correlation ID: {correlationId}. Input: {saleUpdateDto}");
                return this.ToApiResponse<SaleUpdateDto>(ex.Message, 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSale(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var result = await _saleService.DeleteSale(id);
                if (!result)
                {
                    return this.ToApiResponse<bool>("Sale not found", 404);
                }
                return this.ToApiResponse(true, "Sale deleted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting sale with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<bool>(ex.Message, 500);
            }
        }
    }
}
