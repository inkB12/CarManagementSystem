using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.WebMVC.Extensions;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int carId, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];

            var car = await _vehicleService.GetByIdAsync(carId);

            if (car == null)
            {
                return NotFound();
            }

            var existingItem = cart.FirstOrDefault(c => c.CarId.Equals(car.Id));

            if (existingItem != null)
            {
                if (quantity < 0 && quantity > existingItem.Quantity)
                {
                    // Do nothing
                    return RedirectToAction("Index", "Cart");
                }

                existingItem.Quantity += quantity;
                existingItem.Price += car.Price * quantity;
            }
            else
            {
                CartItem cartItem = new()
                {
                    CarId = car.Id,
                    CarName = car.Model,
                    Price = car.Price * quantity,
                    Quantity = quantity,
                    ImageUrl = car.ImageUrl,
                };

                cart.Add(cartItem);
            }



            // Redirect to Index Action in Cart Controller
            return RedirectToAction("Index", "Cart");
        }

    }
}
