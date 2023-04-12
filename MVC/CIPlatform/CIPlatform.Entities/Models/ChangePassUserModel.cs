using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class ChangePassUserModel
    {
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Old Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])).+$", ErrorMessage = "Password must be 8 characters long and must Contain 1-Symbol ,1-lowercase,1-Uppercase,1-digit")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;


        [Required(ErrorMessage = "New Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])).+$", ErrorMessage = "Password must be 8 characters long and must Contain 1-Symbol ,1-lowercase,1-Uppercase,1-digit")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = null!;




        // contact us

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Email-id is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is invalid")]

        public string EmailId { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }



        // edit profile

       
    }
}
