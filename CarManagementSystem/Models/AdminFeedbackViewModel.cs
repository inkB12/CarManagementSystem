using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class AdminFeedbackViewModel
    {
        public int Id { get; set; }

        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Display(Name = "Loại")]
        public string? FeedbackType { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Thời gian")]
        public DateTime Datetime { get; set; }


        // (Tuỳ chọn) Thông tin hiển thị thêm cho admin:
        [Display(Name = "Email")]
        public string? UserEmail { get; set; }
        [Display(Name = "Tên")]
        public string? UserFullName { get; set; }
    }
}
