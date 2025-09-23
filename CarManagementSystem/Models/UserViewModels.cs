using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class UserCreateViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Phone { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Customer";

        public bool IsActive { get; set; } = true;
    }

    public class UserEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Phone { get; set; }

        // Để trống nếu không đổi
        public string? Password { get; set; }

        [Required]
        public string Role { get; set; } = "Customer";

        public bool IsActive { get; set; } = true;
    }
}
