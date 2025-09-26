using System.Threading.Tasks;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Dtos.Momo;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using CarManagementSystem.WebMVC.Models.Checkout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarManagementSystem.WebMVC.Controllers
{
    [Route("[controller]")]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMomoService _momoService;

        public CheckoutController(IOrderService orderService, IMomoService momoService)
        {
            _orderService = orderService;
            _momoService = momoService;
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
                // For test only
                var momoResponse = await _momoService.CreatePaymentAsync(null);

                if (momoResponse != null && !momoResponse.PayUrl.IsNullOrEmpty())
                {
                    return Redirect(momoResponse.PayUrl);
                }

                // Check dữ liệu ko hợp lệ thì trả về View tb lỗi
                return View(model);
            }
            else
            {
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
                return View(model);
            }
        }

        /// <summary>
        /// Momo Callback to BE Action (Momo cannot access localhost so cannot test here)
        /// </summary>
        /// <param name="momoResponse"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("momo-ipn")]
        public async Task<IActionResult> MomoIpn([FromBody] MomoResponseDTO momoResponse)
        {
            if (momoResponse == null)
            {
                return BadRequest();
            }

            if (momoResponse.ResultCode == 0)
            {
                // Succesful Payment
            }

            return Ok();
        }

        /// <summary>
        /// Momo Redirect User to Success/Fail View
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="orderId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("momo-redirect")]
        public async Task<IActionResult> MomoRedirect(int resultCode, string orderId)
        {
            string paymentMessage;

            if (resultCode == 0)
            {
                // Succesful Payment
                paymentMessage = "Đơn hàng thanh toán thành công";
            }
            else
            {
                // Fail Payment
                paymentMessage = "Đơn hàng thanh toán thất bại";
            }

            ViewBag.PaymentMessage = paymentMessage;
            ViewBag.OrderId = orderId;
            ViewBag.ResultCode = resultCode;

            return View();
        }
    }
}
