using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class EditOrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(200, ErrorMessage = "Địa chỉ tối đa 200 ký tự")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Mã bưu điện không được để trống")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Mã bưu điện phải là 6 chữ số")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Mã bưu điện phải từ 4 đến 6 ký tự")]
        public string ZipCode { get; set; } = null!;

        public string? Note { get; set; }
    }
}
