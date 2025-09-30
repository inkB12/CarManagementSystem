using System;
using System.Collections.Generic;

namespace CarManagementSystem.BusinessObjects;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public int OrderId { get; set; }

    public int ElectricVehicleId { get; set; }

    public virtual ElectricVehicle ElectricVehicle { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
