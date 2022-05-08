using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetProducts());
        }

        [HttpGet("{id:length(24)}", Name = "GetProductById")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError($"the product id parameter is required");
                return NotFound();
            }
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                _logger.LogError($"can not find any product with id {id}", id);
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("[action]/{category}")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return NotFound();
            var products = await _productRepository.GetProductByCategory(category);
            if (products == null) return NotFound();
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            if (!string.IsNullOrWhiteSpace(product.Id))
                return CreatedAtRoute("GetProductById", new { id = product.Id }, product);

            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(Product))]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(Product))]
        public async Task<ActionResult> DeleteProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.DeleteProduct(product));
        }
    }
}
