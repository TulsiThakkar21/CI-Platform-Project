using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }


    }
}
