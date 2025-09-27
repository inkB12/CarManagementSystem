

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

        public async Task<(bool ok, string message, Order? data)> CreateAsync(Order order)
        {
            order.Datetime = DateTime.Now;
            order.Status = "PENDING";

            var saved = await _orderRepository.CreateAsync(order);
            return (true, "Order Created", saved);
        }

        public async Task<(bool ok, string message)> DeleteAsync(int orderId)
        {
            await _orderRepository.DeleteAsync(orderId);
            return (true, "Success");
        }

        public async Task<List<Order>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
            {
                return await _orderRepository.GetAllAsync(o => o.Status != "CANCELLED");
            }
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<(bool ok, string message, Order? data)> UpdateAsync(Order order)
        {
            var existedOrder = await GetOrderByIdAsync(order.Id);
            if (existedOrder == null)
            {
                return (false, "Order Not Found", null);
            }

            existedOrder.Status = order.Status ?? existedOrder.Status;
            existedOrder.Address = order.Address ?? existedOrder.Address;
            existedOrder.ZipCode = order.ZipCode ?? existedOrder.ZipCode;
            existedOrder.Note = order.Note ?? existedOrder.Note;

            var updated = await _orderRepository.UpdateAsync(existedOrder);
            return (true, "Success", updated);
        }
    }
}
