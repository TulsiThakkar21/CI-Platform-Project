using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class Admin_MTVM
    {
        public long MissionThemeId { get; set; }


        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }
    }
}
