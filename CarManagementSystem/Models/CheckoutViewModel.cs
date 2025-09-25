using System.ComponentModel.DataAnnotations;
using CarManagementSystem.WebMVC.Models.Checkout;

namespace CarManagementSystem.WebMVC.Models
{
    public class CheckoutViewModel
    {
        public CustomerInfoModel? CustomerInfo { get; set; }
        public AddressInfoModel? AddressInfo { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
        public int? PromotionId { get; set; }
        [Range(typeof(bool), "true", "false", ErrorMessage = "Bạn phải đồng ý với điều khoản và chính sách mới có thể tiếp tục.")]
        public bool TermsAccepted { get; set; }
    }
}
