using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới (bỏ trống nếu không đổi)")]
        public string? Password { get; set; }
    }

}
