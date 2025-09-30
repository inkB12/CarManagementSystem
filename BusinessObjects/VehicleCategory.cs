using System;
using System.Collections.Generic;

namespace CarManagementSystem.BusinessObjects;

public partial class VehicleCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<ElectricVehicle> ElectricVehicles { get; set; } = new List<ElectricVehicle>();
}
