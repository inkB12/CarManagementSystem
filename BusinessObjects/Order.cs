using System;
using System.Collections.Generic;

namespace CarManagementSystem.BusinessObjects;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Datetime { get; set; }

    public decimal Total { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public int UserId { get; set; }

    public string? Note { get; set; }

    public int? PromotionId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Promotion? Promotion { get; set; }

    public virtual User User { get; set; } = null!;
}
