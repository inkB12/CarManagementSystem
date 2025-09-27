using System.ComponentModel.DataAnnotations;
using CarManagementSystem.WebMVC.Models.Cart;
using CarManagementSystem.WebMVC.Models.Checkout;

namespace CarManagementSystem.WebMVC.Models
{
    public class CheckoutViewModel
    {
        public CustomerInfoModel? CustomerInfo { get; set; } = new();
        public AddressInfoModel? AddressInfo { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = [];
        public decimal TotalPrice { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
        public int? PromotionId { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản và chính sách mới có thể tiếp tục.")]
        public bool TermsAccepted { get; set; }
    }
}
