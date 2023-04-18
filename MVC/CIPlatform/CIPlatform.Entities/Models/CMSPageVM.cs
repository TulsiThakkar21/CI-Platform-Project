using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class CMSPageVM
    {
        public long CmsPageId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Slug is required")]
        public string Slug { get; set; } = null!;

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
