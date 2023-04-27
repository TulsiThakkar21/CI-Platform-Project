using Foolproof;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class AdminMissionVM
    {
        public long MissionId { get; set; }

        [Required(ErrorMessage = "Please select a city")]
        public string CityId { get; set; }

        [Required(ErrorMessage = "Please select a country")]
        public long CountryId { get; set; }

        [Required(ErrorMessage = "Please select a theme")]
        public string ThemeId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Short Description is required")]
        public string ShortDescription { get; set; } = null!;

        [Required(ErrorMessage = "File is required")]
        public List<IFormFile> File { get; set; } = null!;

        [Required(ErrorMessage = "Document File is required")]
        public List<IFormFile> DocumentFile { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Selected Values is required")]
        public List<string> SelectedValues { get; set; } = null!;


        
        [DataType(DataType.Date)]
        [Display(Name = "StartDate")]
        //[GreaterThan(nameof(Today), ErrorMessage = "The {0} field must be greater than today's date.")]
        [Required(ErrorMessage = "Start Date is required")]
        [DateGreaterThanToday]
        public DateTime StartDate { get; set; } 

        public DateTime Today => DateTime.Today;


        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "EndDate")]
        [DateGreaterThanToday]
        //[GreaterThan(nameof(Today), ErrorMessage = "The {0} field must be greater than today's date.")]
        public DateTime EndDate { get; set; }
        

        [Required(ErrorMessage = "Mission Type is required")]
        public string MissionType { get; set; } = null!;

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "Organization Name is required")]
        public string OrganizationName { get; set; } = null!;

        [Required(ErrorMessage = "Organization Detail is required")]
        public string OrganizationDetail { get; set; } = null!;

        [Required(ErrorMessage = "Availability is required")]
        public string Availability { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime DeletedAt { get; set; }

        [Required(ErrorMessage = "Mission Availability is required")]
        public string MissionAvailability { get; set; } = null!;

        public virtual City City { get; set; } = null!;
    }


    public class DateGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime inputDate = (DateTime)value;

            if (inputDate.Date <= DateTime.Now.Date)
            {
                return new ValidationResult("The date must be greater than today's date.");
            }

            return ValidationResult.Success;
        }
    }
}
