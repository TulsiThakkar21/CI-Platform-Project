using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }


    }
}
