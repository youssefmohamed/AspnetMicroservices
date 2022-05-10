using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Dapper;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string _connectionString;
        public DiscountRepository(IConfiguration configuration)
        {
            _connectionString = configuration["DatabaseSettings:ConnectionString"];
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var createResult = await connection.ExecuteAsync
                ("insert into coupon(productid, description, amount) values(@ProductId, @Description, @Amount)", new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = coupon.Amount });

            return createResult > 0;
        }

        public async Task<bool> DeleteDiscount(string productId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var deleteResult = await connection.ExecuteAsync("delete from coupon where productid = @productId", new { productId = productId });
            return deleteResult > 0;
        }

        public async Task<Coupon> GetDiscount(string productId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from coupon where productid=@productId", new { productId = productId });
            return coupon ?? new Coupon { Amount = 0 };
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var updateResult = await connection.ExecuteAsync("update coupon set productid=@ProductId, description=@Description, amount=@Amount", new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = coupon.Amount });
            return updateResult > 0;
        }
    }
}
