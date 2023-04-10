using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models;

public partial class PasswordReset
{
    public int PassResetId { get; set; }

    public string? Email { get; set; }

    public string? Token { get; set; }

    public DateTime CreatedAt { get; set; }


}
