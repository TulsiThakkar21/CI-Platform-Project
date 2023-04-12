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
        [Required(ErrorMessage = "Please select the mission.")]
        public string Mission {get; set;} = null!;

        public DateTime Date { get; set; }

        public TimeOnly Hours { get; set; } 

        public TimeOnly Minutes { get; set; }

        public string Message { get; set; }

    }
}
