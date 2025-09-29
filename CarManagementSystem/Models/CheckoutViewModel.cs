using System.ComponentModel.DataAnnotations;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.WebMVC.Models.Cart;
using CarManagementSystem.WebMVC.Models.Checkout;

namespace CarManagementSystem.WebMVC.Models
{
    public class CheckoutViewModel
    {
        public AddressInfoModel? AddressInfo { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = [];
        public decimal TotalPrice { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
        public Promotion? Promotion { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản và chính sách mới có thể tiếp tục.")]
        public bool TermsAccepted { get; set; }

        // For select
        public List<Promotion> Promotions { get; set; } = new();
        public int ApplyPromotionId { get; set; }

    }
}
