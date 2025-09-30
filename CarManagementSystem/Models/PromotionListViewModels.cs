// PromotionListVm.cs
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarManagementSystem.BusinessObjects;

namespace CarManagementSystem.WebMVC.Models
{
    public class PromotionListViewModels
    {
        // filter
        public string? Q { get; set; }
        public string? Status { get; set; } // "Active"/"Inactive"/null
        public List<SelectListItem> StatusOptions { get; set; } = new()
        {
            new("— Tất cả —",""),
            new("Active","Active"),
            new("Inactive","Inactive")
        };

        // paging
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        // data
        public List<Promotion> Items { get; set; } = new();
    }


  
}
