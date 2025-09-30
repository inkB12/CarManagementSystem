using CarManagementSystem.WebMVC.Extensions;
using CarManagementSystem.WebMVC.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        // Not DI service, Just Access Session
        public IViewComponentResult Invoke()
        {
            // 1. Get cart from session
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];

            // 2. Return View
            return View("Default", cart.Count);
        }
    }
}
