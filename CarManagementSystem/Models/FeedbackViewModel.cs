using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Loại phản hồi")]
        public string FeedbackType { get; set; } = "";

        [Required]
        [StringLength(2000)]
        [Display(Name = "Nội dung")]
        public string Content { get; set; } = "";

        public DateTime Datetime { get; set; }

        public string UserName { get; set; } = "";
    }
}
