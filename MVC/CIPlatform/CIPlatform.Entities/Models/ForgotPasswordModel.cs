using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email-id is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]  
        public string Email { get; set; }


    }
}
