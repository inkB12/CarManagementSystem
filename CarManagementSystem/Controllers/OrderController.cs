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

        [HttpGet]
        public async Task<IActionResult> EditOrder(int orderId)
        {
            // Kiểm tra đăng nhập
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null || order.UserId != userId)
            {
                return NotFound();
            }

            return View(new EditOrderViewModel()
            {
                Id = order.Id,
                Address = order.Address,
                ZipCode = order.ZipCode,
                Note = order.Note,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(EditOrderViewModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            // Kiểm tra đăng nhập
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var existingOrder = await _orderService.GetOrderByIdAsync(editModel.Id);
            existingOrder.Address = editModel.Address;
            existingOrder.ZipCode = editModel.ZipCode;
            existingOrder.Note = editModel.Note;

            var (ok, message, _) = await _orderService.UpdateAsync(existingOrder);

            if (ok)
            {
                return RedirectToAction("UserOrder", "Order", new { id = userId });
            }

            ModelState.AddModelError(string.Empty, $"Cập nhật thất bại: {message}");
            return View(existingOrder);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _orderService.GetOrderByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new OrderViewModel
            {

                Id = entity.Id,
                Datetime = entity.Datetime,
                Total = entity.Total,
                PaymentMethod = entity.PaymentMethod,
                Status = entity.Status,
                Address = entity.Address,
                ZipCode = entity.ZipCode,
                Note = entity.Note,
            };

            return View(vm);
        }

        // POST: Orders/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, message) = await _orderService.DeleteAsync(id);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }
    }
}
