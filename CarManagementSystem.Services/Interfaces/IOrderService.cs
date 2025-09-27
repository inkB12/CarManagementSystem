

using CarManagementSystem.BusinessObjects;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<(bool ok, string message, Order? data)> CreateAsync(Order order);
        Task<(bool ok, string message, Order? data)> UpdateAsync(Order order);
        Task<(bool ok, string message)> DeleteAsync(int orderId);
        Task<List<Order>> GetAllAsync(bool onlyActive = true);
    }
}
