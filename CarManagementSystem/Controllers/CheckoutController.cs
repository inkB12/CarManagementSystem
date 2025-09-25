using System.Threading.Tasks;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using CarManagementSystem.WebMVC.Models.Checkout;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    [Route("[controller]")]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;

        public CheckoutController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Checkout()
        {
            // 1. Create ViewModel
            CheckoutViewModel model = new()
            {
                CustomerInfo = new CustomerInfoModel()
            };

            // 2. Pass to View
            return View(model);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Check dữ liệu ko hợp lệ thì trả về View tb lỗi
                return View(model);
            }

            // Create Order
            var response = await _orderService.CreateAsync(new Order()
            {
                PaymentMethod = model.PaymentMethod,
                Address = model.AddressInfo.Address,
                ZipCode = model.AddressInfo.ZipCode,
                Note = model.AddressInfo.Note,
                PromotionId = model.PromotionId,
            });

            if (response.ok)
            {
                var createdOrder = response.data;
                var paymentUrl = "url";
            }

            

            return Redirect(paymentUrl);
        }
    }
}
