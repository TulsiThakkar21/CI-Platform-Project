using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class Admin_MSkillsVM
    {
        public long MissionSkillId { get; set; }

        public long SkillId { get; set; }

        public long MissionId { get; set; }

        public string MissionTitle { get; set; }

        public string Skillname { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }

        public virtual Mission Mission { get; set; }

        public virtual Skill Skill { get; set; } = null!;
    }
}
