

using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<(bool ok, string message, Order? data)> CreateAsync(Order order);
    }
}
