using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class Admin_SkillsVM
    {
        public long SkillsId { get; set; }


        [Required(ErrorMessage = "Skill Name is required")]
        public string SkillName { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
