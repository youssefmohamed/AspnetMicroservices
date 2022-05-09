using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productId}", Name = "GetDiscount")]
        public async Task<ActionResult<Coupon>> GetDiscount(string productId) 
        {
            return Ok(await _discountRepository.GetDiscount(productId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(Coupon coupon)
        {
            return Ok(await _discountRepository.CreateDiscount(coupon));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCoupon(Coupon coupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(coupon));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCoupon(string productId) 
        {
            return Ok(await _discountRepository.DeleteDiscount(productId));
        }
    }
}
