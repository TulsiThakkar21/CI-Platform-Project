using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models
{ 

    public class LoginModel
    {

        [Required(ErrorMessage = "Email-id is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string LoginId { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])).+$", ErrorMessage = "Password must be 8 characters long and must Contain 1-Symbol ,1-lowercase,1-Uppercase,1-digit")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
