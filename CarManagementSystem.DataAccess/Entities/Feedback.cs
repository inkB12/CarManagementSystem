using System;
using System.Collections.Generic;

namespace CarManagementSystem.DataAccess;

public partial class Feedback
{
    public int Id { get; set; }

    public string FeedbackType { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Datetime { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
