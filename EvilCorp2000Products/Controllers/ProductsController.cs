using Asp.Versioning;
using AutoMapper;
using EvilCorp2000Products.DbContexts;
using EvilCorp2000Products.Models;
using EvilCorp2000Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace EvilCorp2000Products.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;
        const int maxProductsPageSize = 10;

        public ProductsController(ILogger<ProductsController> logger, IMapper mapper, IProductRepository productRepo)
        { 
            _logger = logger ?? throw new ArgumentNullException (nameof(logger));
            this._mapper = mapper;
            this._productRepo = productRepo;
        }


        /// <param name="productClass">Available product classes are: Furniture, Wearable, Facility, Consumable, Scrolls, Weapons</param>
        [HttpGet]
        [ApiVersion(1)]
        //Action Result: not tied to json + use the built in StatusCode Functions
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(string? productClass, string? searchquery, int currentPageNumber = 1, int pageSize = 4)
        {
            if (pageSize > maxProductsPageSize) { pageSize = maxProductsPageSize; }

            try
            {
                var (productsEntities, paginationMetaData) = await _productRepo.GetProducts(productClass, searchquery, currentPageNumber, pageSize);

                Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetaData);

                return Ok(_mapper.Map<IEnumerable<ProductDTO>>(productsEntities));
            }
            catch (Exception ex) 
            {
                _logger.LogCritical($"Exception while getting Products {ex}");
                return StatusCode(500, "A problem occured while handling your request");
            }
        }


        [HttpGet("{id}", Name = "GetProductById")]
        [ApiVersion(2)]        
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null) 
                {
                    _logger.LogInformation($"Product {id} was not found");
                    return NotFound(); 
                }
                return Ok(_mapper.Map<ProductDTO>(product));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Product {id}", ex);
                return StatusCode(500, "A problem occured while handling your request");
            }
        }


        [HttpPost]
        [ApiVersion(1)]
        [Authorize]
        public async Task <ActionResult<ProductDTO>> CreateNewProduct(ProductForCreationDto productForCreation)
        {
            try 
            {
                var newProduct = _mapper.Map<Entities.Product>(productForCreation);

                _productRepo.CreateNewProduct(newProduct);
                await _productRepo.SaveChangesAsync();

                var productDtoToReturn = _mapper.Map<ProductDTO>(newProduct);

                return CreatedAtRoute(
                    "GetProductById",
                    new { id = productDtoToReturn.ProductId },
                    productDtoToReturn
                    );
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while creating new Product {productForCreation}", ex);
                return StatusCode(500, "A problem occured while handling your request");
            }
        }


        [HttpPut("{productId}")]
        [Authorize]
        [ApiVersion(1)]
        public async Task<ActionResult> UpdateProduct(int productId, ProductForUpdateDto product)
        {
            try 
            {
                //ist produkt da
                var productEntity = await _productRepo.GetProductById(productId);

                //wenn nicht...
                if (productEntity == null) 
                { 
                    _logger.LogInformation($"Product {productId} was not found");
                    return NotFound("product not found");
                };

                //wenn ja map for update zu product und speichere

                _mapper.Map(product, productEntity);

                await _productRepo.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while updating Product {product} with productId {productId}", ex);
                return StatusCode(500, "A problem occured while handling your request");
            }
        }


        [HttpPatch("{productId}")]
        [Authorize]
        [ApiVersion(1)]
        public async Task<ActionResult> PartiallyUpdateProduct(int productId, JsonPatchDocument<ProductForUpdateDto> product)
        {
            try
            {
                var productEntity = await _productRepo.GetProductById(productId);

                if (productEntity == null)
                {
                    _logger.LogInformation($"Product {productId} was not found");
                    return NotFound("product not found");
                };

                //create new productForUpdate from listedProduct on which the JsonPatchDocument<ProductForUpdateDto> can be applied
                var productForUpdade = _mapper.Map<ProductForUpdateDto>(productEntity);

                //pass in the modelstate to be able to validdate if
                //JsonPatchDocument<ProductForUpdate> only contains valid properties
                product.ApplyTo(productForUpdade, ModelState);
                //validates inputted JsonPatchDocument<ProductForUpdate>
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                //validates newProduct after applying JsonPatchDocument
                if (!TryValidateModel(productForUpdade)) return BadRequest(ModelState);

                _mapper.Map(productForUpdade, productEntity);

                await _productRepo.SaveChangesAsync();
                
                return NoContent();
            }
            

            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while updating Product {product} with productId {productId}", ex);
                return StatusCode(500, "A problem occured while handling your request");
            }
        }


        [HttpDelete]
        [Authorize]
        [ApiVersion(1)]
        public async Task <ActionResult> DeleteProduct(int productId)
        {
            try
            {
                var productEntity = await _productRepo.GetProductById(productId);

                if (productEntity == null)
                {
                    _logger.LogInformation($"Product {productId} was not found");
                    return NotFound("product not found");
                };

                _productRepo.DeleteProduct(productEntity);
                await _productRepo.SaveChangesAsync();

                return NoContent();
            }
            
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while deleting Product with productId {productId}", ex);
                return StatusCode(500, "A problem occured while handling your request");
            }
        }
}

}