using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Extensions;
using CarManagementSystem.WebMVC.Models;
using CarManagementSystem.WebMVC.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IElectricVehicleService _vehicleService;

        public CartController(IElectricVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public IActionResult MockCart()
        {
            var mockCartItems = new List<CartItem>
        {
            new CartItem
            {
                CarId = 1,
                CarName = "Xe Điện Takashi Mono - Đen bóng",
                Price = 13990000,
                Quantity = 1,
                ImageUrl = "/images/car_mock_1.jpg" // Đổi đường dẫn ảnh thực tế của bạn
            },
            new CartItem
            {
                CarId = 2,
                CarName = "Xe Máy Điện Takashi Crown - Đen",
                Price = 14500000,
                Quantity = 2, // Thử với số lượng 2
                ImageUrl = "/images/car_mock_2.jpg"
            }
        };

            // Lưu giỏ hàng giả vào Session với key "Cart"
            HttpContext.Session.Set("Cart", mockCartItems);

            // Chuyển hướng người dùng đến trang hiển thị giỏ hàng chính thức
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            // 1. Get existed cart
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];

            decimal totalPrice = cart.Sum(x => x.Price * x.Quantity);

            // 2. Create View Model
            CartViewModel cartViewModel = new()
            {
                CartItems = cart,
                TotalPrice = totalPrice
            };

            return View(cartViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int carId, int quantity)
        {

            // 1. Get existed cart
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];

            // 2. Get existed car from cart
            var car = await _vehicleService.GetByIdAsync(carId);

            // Car ko có
            if (car == null)
            {
                return NotFound();
            }

            // 3. Get existed item in cart if any
            var existingItem = cart.FirstOrDefault(c => c.CarId.Equals(car.Id));

            if (existingItem != null)
            {
                // Add quantity
                int tempQuantity = existingItem.Quantity + quantity;

                if (tempQuantity <= 0)
                {
                    cart.Remove(existingItem);
                }

                existingItem.Quantity = tempQuantity;
            }
            else
            {
                // Add new one
                CartItem cartItem = new()
                {
                    CarId = car.Id,
                    CarName = car.Model,
                    Price = car.Price,
                    Quantity = quantity,
                    ImageUrl = car.ImageUrl,
                };

                cart.Add(cartItem);
            }

            // Update Cart
            HttpContext.Session.Set("Cart", cart);

            // Redirect to Index Action in Cart Controller
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult UpdateCart(int carId, int quantity)
        {
            // 1. Get existed cart
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];

            // 2. Get existed car from cart
            var existingItem = cart.FirstOrDefault(c => c.CarId == carId);

            if (existingItem != null)
            {
                int tempQuantity = existingItem.Quantity + quantity;

                if (tempQuantity <= 0)
                {
                    cart.Remove(existingItem);
                }

                existingItem.Quantity = tempQuantity;
            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public IActionResult RemoveCartItem(int carId)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];

            var existedItem = cart.FirstOrDefault(c => c.CarId == carId);
            if (existedItem != null)
            {
                cart.Remove(existedItem);
            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Cart");
        }
    }
}
