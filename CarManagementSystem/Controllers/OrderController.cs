using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> UserOrder(int id)
        {
            List<OrderViewModel> orderViewModels = [];

            if (id != null && id != 0)
            {
                var orders = await _orderService.GetOrderByUserIdAsync((int)id);
                foreach (var order in orders)
                {
                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        Datetime = order.Datetime,
                        Total = order.Total,
                        PaymentMethod = order.PaymentMethod,
                        Status = order.Status,
                        Address = order.Address,
                        ZipCode = order.ZipCode,
                        Note = order.Note,
                        Promotion = order.Promotion,
                        OrderDetails = [.. order.OrderDetails],
                        User = order.User
                    });
                }
            }

            return View(orderViewModels);
        }
    }
}
