using System;
using System.Collections.Generic;

namespace CarManagementSystem.DataAccess;

public partial class Promotion
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Discount { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
