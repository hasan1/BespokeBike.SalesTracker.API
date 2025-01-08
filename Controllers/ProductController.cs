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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductGetDto>>>> GetAllProducts()
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var products = await _productService.GetAllProducts();
                var productDtos = _mapper.Map<IEnumerable<ProductGetDto>>(products);
                return this.ToApiResponse(productDtos, "Products retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving products. Correlation ID: {correlationId}");
                return this.ToApiResponse<IEnumerable<ProductGetDto>>(ex.Message, 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductGetDto>>> GetProductById(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return this.ToApiResponse<ProductGetDto>("Product not found", 404);
                }
                var productDto = _mapper.Map<ProductGetDto>(product);
                return this.ToApiResponse(productDto, "Product retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving product with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<ProductGetDto>(ex.Message, 500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Product>>> AddProduct([FromBody] ProductCreateDto productCreateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var product = _mapper.Map<Product>(productCreateDto);
                var result = await _productService.AddProduct(product);
                return this.ToApiResponse(result, "product created successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding a new product. Correlation ID: {correlationId}. Input: {productCreateDto}");
                return this.ToApiResponse<Product>(ex.Message, 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                if (id != productUpdateDto.ProductId)
                {
                    return this.ToApiResponse<Product>("Product ID mismatch", 400);
                }

                var product = _mapper.Map<Product>(productUpdateDto);
                var result =  await _productService.UpdateProduct(product);
                return this.ToApiResponse(result, "product updated successfully", 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {id}. Correlation ID: {correlationId}. Input: {productUpdateDto}");
                return this.ToApiResponse<Product>(ex.Message, 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
        {
            var correlationId = ApiResponseExtensions.GetCorrelationId(this);
            try
            {
                var result = await _productService.DeleteProduct(id);
                if (!result)
                {
                    return this.ToApiResponse<bool>("Product not found", 404);
                }
                return this.ToApiResponse(true, "Product deleted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {id}. Correlation ID: {correlationId}");
                return this.ToApiResponse<bool>(ex.Message, 500);
            }
        }
    }
}

