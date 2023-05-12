using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public long UserId { get; set; }

    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreatedOn { get; set; }

    public long? MissionId { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual User User { get; set; } = null!;
}
