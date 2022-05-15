using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Presistense;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IEmailSender emailSender, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _emailSender = emailSender;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {orderEntity.Id} is placed successfully");
            await SendEmailAsync(orderEntity.Id);
            return orderEntity.Id;

        }

        private async Task SendEmailAsync(int orderId)
        {
            var email = new Email
            {
                To = "youssefmohamed04@gmail.com",
                Subject = "Order checkout",
                Body = $"Order {orderId} is placed successfully"
            };
            await _emailSender.SendEmailAsync(email);
        }
    }
}
