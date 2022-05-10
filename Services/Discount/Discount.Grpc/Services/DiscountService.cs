using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(ProductIdRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductId);
            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CouponModel request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request);
            await _discountRepository.CreateDiscount(coupon);
            return request;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(ProductIdRequest request, ServerCallContext context)
        {
            var deleteResult = await _discountRepository.DeleteDiscount(request.ProductId);
            return new DeleteDiscountResponse { Success = deleteResult };
        }

        public override async Task<CouponModel> UpdateDiscount(CouponModel request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request);
            await _discountRepository.UpdateDiscount(coupon);
            return request;
        }
    }
}
