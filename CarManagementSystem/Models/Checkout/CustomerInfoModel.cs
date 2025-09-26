using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models.Checkout
{
    public class CustomerInfoModel
    {
        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required, Phone]
        public string? Phone { get; set; }
    }
}
