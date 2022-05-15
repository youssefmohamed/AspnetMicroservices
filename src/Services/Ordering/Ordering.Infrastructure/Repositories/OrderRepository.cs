using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Presistense;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Presistense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrderByUserNameAsync(string username)
        {
            return await _orderContext.Orders.Where(x => x.UserName == username).ToListAsync();
        }
    }
}
