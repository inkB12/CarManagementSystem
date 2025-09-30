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

        public async Task<IActionResult> UserOrder()
        {
            // Kiểm tra đăng nhập
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<OrderViewModel> orderViewModels = [];

            if (userId != null && userId != 0)
            {
                var orders = await _orderService.GetOrderByUserIdAsync((int)userId);
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


        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) || string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<IActionResult> AllOrders()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            // Kiểm tra đăng nhập
            int? userId = HttpContext.Session.GetInt32("UserId");
            string? userRole = HttpContext.Session.GetString("UserRole") ?? "Customer";

            if (userId == null || userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<OrderViewModel> orderViewModels = [];

            var orders = await _orderService.GetAllAsync();
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

            ViewBag.Layout = GetLayout(userRole);

            return View(orderViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int orderId)
        {
            // Kiểm tra đăng nhập
            int? userId = HttpContext.Session.GetInt32("UserId");
            string? userRole = HttpContext.Session.GetString("UserRole") ?? "Customer";

            if (userId == null || userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null || (order.UserId != userId && userRole.Equals("Customer")))
            {
                return NotFound();
            }

            ViewBag.Layout = GetLayout(userRole);

            return View(new EditOrderViewModel()
            {
                Id = order.Id,
                Address = order.Address,
                ZipCode = order.ZipCode,
                Note = order.Note,
                Status = order.Status,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(EditOrderViewModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            var existingOrder = await _orderService.GetOrderByIdAsync(editModel.Id);
            existingOrder.Address = editModel.Address;
            existingOrder.ZipCode = editModel.ZipCode;
            existingOrder.Note = editModel.Note;
            existingOrder.Status = editModel.Status;

            var (ok, message, _) = await _orderService.UpdateAsync(existingOrder);

            if (ok)
            {
                string? userRole = HttpContext.Session.GetString("UserRole") ?? "Customer";
                (string action, string control) = GetAction(userRole);
                return RedirectToAction(action, control);
            }

            ModelState.AddModelError(string.Empty, $"Cập nhật thất bại: {message}");
            return View(existingOrder);
        }

        // GET: Order/Delete/5
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

        // POST: Order/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, message) = await _orderService.DeleteAsync(id);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            string? userRole = HttpContext.Session.GetString("UserRole") ?? "Customer";
            (string action, string control) = GetAction(userRole);
            return RedirectToAction(action, control);
        }

        private string GetLayout(string userRole)
        {
            string layout = userRole switch
            {
                "Customer" => "~/Views/Shared/_Layout.cshtml",
                "Admin" => "~/Views/Shared/_LayoutAdmin.cshtml",
                _ => "~/Views/Shared/_LayoutStaff.cshtml",
            };
            return layout;
        }

        private (string action, string controller) GetAction(string userRole)
        {
            (string action, string controller) result = userRole switch
            {
                "Customer" => ("UserOrder", "Order"),
                "Admin" => ("AllOrders", "Order"),
                _ => ("AllOrders", "Order"),
            };
            return result;
        }
    }
}
