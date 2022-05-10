using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services.Grpc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketController> _logger;
        private readonly DiscountGrpcService _discountGrpcService;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
            _discountGrpcService = discountGrpcService; 
        }

        [HttpGet("{username}")]
        [ProducesResponseType(statusCode:(int)HttpStatusCode.OK, Type = typeof(ShoppingCart))]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username) 
        {
            var basket = await _basketRepository.GetBasket(username);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            foreach (var item in shoppingCart.Items)
            {
                var discount = await _discountGrpcService.GetDiscount(item.ProductId);
                item.Price -= discount.Amount;
            }
            return await _basketRepository.UpdateBaskeet(shoppingCart);
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string username) 
        {
            await _basketRepository.DeleteBasket(username);
            return Ok();
        }
    }
}
