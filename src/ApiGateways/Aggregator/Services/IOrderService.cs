using Aggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aggregator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
