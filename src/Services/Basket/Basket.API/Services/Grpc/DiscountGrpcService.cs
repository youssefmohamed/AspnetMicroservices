using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.Services.Grpc
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> GetDiscount(string productId)
        {
            var productIdRequest = new ProductIdRequest { ProductId = productId };
            return await _discountProtoService.GetDiscountAsync(productIdRequest);
        }
    }
}
