using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.Models;

public partial class ContactU
{
    public int ContactUsId { get; set; }

    public long UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
