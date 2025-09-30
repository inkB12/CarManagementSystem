using System.Threading.Tasks;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Dtos.Momo;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Extensions;
using CarManagementSystem.WebMVC.Models;
using CarManagementSystem.WebMVC.Models.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarManagementSystem.WebMVC.Controllers
{
    [Route("[controller]")]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMomoService _momoService;
        private readonly IPromotionService _promotionService;

        public CheckoutController(IOrderService orderService, IMomoService momoService, IPromotionService promotionService)
        {
            _orderService = orderService;
            _momoService = momoService;
            _promotionService = promotionService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Checkout(int promotionId = 0)
        {
            // 0 Validate Cart
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // 1. Create ViewModel
            var totalPrice = cart.Sum(c => c.Quantity * c.Price);
            Promotion? promotion = null;

            if (promotionId != 0)
            {
                promotion = await _promotionService.GetByIdAsync(promotionId);
                totalPrice -= promotion == null ? 0 : promotion.Discount * totalPrice / 100;
            }

            CheckoutViewModel model = new()
            {
                CartItems = cart,
                TotalPrice = totalPrice,
                Promotion = promotion,
                Promotions = await _promotionService.GetAllAsync(true),
                ApplyPromotionId = promotionId
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
            else
            {
                // Get Cart Item List
                var cartList = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];
                if (cartList.Count == 0)
                {
                    // Back to Cart
                    return RedirectToAction("Index", "Cart");
                }
                decimal total = cartList.Sum(c => c.Quantity * c.Price);

                var orderDetails = new List<OrderDetail>();
                foreach (var item in cartList)
                {
                    orderDetails.Add(new OrderDetail()
                    {
                        Quantity = item.Quantity,
                        TotalPrice = item.Price * item.Quantity,
                        ElectricVehicleId = item.CarId
                    });
                }

                // Get User Id
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null || userId == 0)
                {
                    //Back to Login
                    return RedirectToAction("Login", "Auth");
                }

                // Get Promotion
                Promotion? promotion = await _promotionService.GetByIdAsync(model.ApplyPromotionId);

                // Create Order
                var (ok, _, data) = await _orderService.CreateAsync(new Order()
                {
                    Total = model.TotalPrice,
                    OrderDetails = orderDetails,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.AddressInfo.Address,
                    ZipCode = model.AddressInfo.ZipCode,
                    Note = model.AddressInfo.Note,
                    Promotion = promotion,
                    UserId = (int)userId
                });

                if (ok)
                {
                    HttpContext.Session.Set<List<CartItem>>("Cart", []);

                    switch (model.PaymentMethod)
                    {
                        case "Momo":
                            // Create Payment Url
                            var momoResponse = await _momoService.CreatePaymentAsync(data);

                            if (momoResponse != null && !momoResponse.PayUrl.IsNullOrEmpty())
                            {
                                return Redirect(momoResponse.PayUrl);
                            }
                            break;

                        default:
                            // Redirect to order history
                            return RedirectToAction("UserOrder", "Order", new { id = userId });
                    }


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
            string status = "CANCELLED";
            string subStringId = orderId[35..];

            if (resultCode == 0)
            {
                // Succesful Payment
                paymentMessage = "Đơn hàng thanh toán thành công";
                status = "SUCCESS";
            }
            else
            {
                // Fail Payment
                paymentMessage = "Đơn hàng thanh toán thất bại";
            }

            if (int.TryParse(subStringId, out int id))
            {
                await _orderService.UpdateAsync(new Order()
                {
                    Id = id,
                    Status = status
                });
            }
            else
            {
                paymentMessage = "Đơn hàng thanh toán thất bại do mã đơn lỗi";
            }

            ViewBag.PaymentMessage = paymentMessage;
            ViewBag.OrderId = id;
            ViewBag.ResultCode = resultCode;

            return View();
        }
    }
}
