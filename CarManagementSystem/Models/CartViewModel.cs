using CarManagementSystem.WebMVC.Models.Cart;

namespace CarManagementSystem.WebMVC.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; } = [];
        public decimal TotalPrice { get; set; }
    }
}
