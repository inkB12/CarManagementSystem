

using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;

namespace CarManagementSystem.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Create Order
        public async Task<(bool ok, string message, Order? data)> CreateAsync(Order order)
        {
            order.Datetime = DateTime.Now;
            order.Status = "PENDING";

            var saved = await _orderRepository.CreateAsync(order);
            return (true, "Order Created", saved);
        }
    }
}
