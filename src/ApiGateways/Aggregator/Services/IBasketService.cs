using Aggregator.Models;
using System.Threading.Tasks;

namespace Aggregator.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
    }
}
