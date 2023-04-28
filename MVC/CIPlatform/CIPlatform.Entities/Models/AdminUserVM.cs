using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class AdminUserVM
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


        //[Required(ErrorMessage = "CityId is required")]
        public string CityId { get; set; } = null!;


        //[Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; } = null!;

        public string Avatar { get; set; } 

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }



        [MinLength(6, ErrorMessage = "Employee Id must be at least 6 characters long")]
        [StringLength(6, ErrorMessage = "Max 6 digits are valid")]
        public string empidEdit { get; set; }
        public string Department { get; set; }
        public string EmployeeId { get; set; }
        public string ProfileText { get; set; }
        public long uid{ get; set; }
    }
}