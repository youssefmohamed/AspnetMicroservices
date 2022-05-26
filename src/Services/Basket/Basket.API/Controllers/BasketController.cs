using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services.Grpc;
using EventBus.Messages.Events;
using MassTransit;
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
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, ILogger<BasketController> logger, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _logger = logger;
            _discountGrpcService = discountGrpcService; 
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
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

        [HttpPost("[action]")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
                return BadRequest();

            var eventMsg = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            await _publishEndpoint.Publish(eventMsg);

            await _basketRepository.DeleteBasket(basketCheckout.UserName);

            return Accepted();
        }
    }
}
