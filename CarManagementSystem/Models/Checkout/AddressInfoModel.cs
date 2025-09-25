using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models.Checkout
{
    public class AddressInfoModel
    {
        [Required]
        public string? Address { get; set; }

        [Required, MinLength(6)]
        public string? ZipCode { get; set; }
        public string? Note { get; set; }
    }
}
