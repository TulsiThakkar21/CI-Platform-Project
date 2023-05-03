using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email-id is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is invalid")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])).+$", ErrorMessage = "Password must be 8 characters long and must Contain 1-Symbol ,1-lowercase,1-Uppercase,1-digit")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Display(Name = "PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]

        public string PhoneNumber { get; set; }


        public long CityId { get; set; }

        public long CountryId { get; set; }

    }
}
