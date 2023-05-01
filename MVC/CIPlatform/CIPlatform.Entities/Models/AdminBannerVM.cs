using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class AdminBannerVM
    {
        public long BannerId { get; set; }

        public string Image { get; set; } = null!;

        public string Text { get; set; }

        public int SortOrder { get; set; }

        public IFormFile formFile { get; set; }

        public string ImageName { get; set; }

    }
}
