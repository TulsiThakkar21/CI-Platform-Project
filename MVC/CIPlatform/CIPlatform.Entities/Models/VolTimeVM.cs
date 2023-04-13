using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class VolTimeVM
    {
        [Required(ErrorMessage = "Please select the mission")]
        public string Mission { get; set; } = null!;

        //public Mission MissionList { set; get; }


        [Required(ErrorMessage = "Please select a date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please mention hours")]
        [Range(0, 23, ErrorMessage = "Hours can only be between 0 to 23")]
        [StringLength(2, ErrorMessage = "Max 2 digits are valid")]
        [DataType(DataType.Time)]
        public TimeOnly Hours { get; set; }


        [Required(ErrorMessage = "Please mention minutes")]
        [Range(0, 59, ErrorMessage = "Minutes can only be between 0 to 59")]
        [StringLength(2, ErrorMessage = "Max 2 digits are valid")]
        [DataType(DataType.Time)]
        public TimeOnly Minutes { get; set; }

        [Required(ErrorMessage = "Please enter some message")]
        [DataType(DataType.MultilineText)]
        [MaxLength(100, ErrorMessage = "Max Length of 100 characters reached")]
        public string Message { get; set; }


        
        // for goal based
        [Required(ErrorMessage = "Please select the mission")]
        public string goalmissionlist { get; set; }

        [Required(ErrorMessage = "Please add your action")]
        //[Range(0, 1, ErrorMessage = "Action should be either 0 or 1")]
        public string Action { get; set; }

        
        [Required(ErrorMessage = "Please select a date")]
        [DataType(DataType.DateTime)]
        public DateTime DateVol { get; set; }

        [Required(ErrorMessage = "Please enter your message")]
        [DataType(DataType.MultilineText)]
        [MaxLength(100, ErrorMessage = "Max Length of 100 characters reached")]
        public string GoalMessage { get; set; }


    }
}
