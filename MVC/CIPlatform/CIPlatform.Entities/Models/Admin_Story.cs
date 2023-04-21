using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class Admin_Story
    {
        public long StoryId { get; set; }

        public long MissionId { get; set; }

        public long UserId { get; set; }

        public string FullName { get; set; }

        public string StoryTitle { get; set; }
        
        public string MissionTitle { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime PublishedAt { get; set; }

        public string? VidUrl { get; set; }

        public virtual Mission Mission { get; set; } = null!;

        public virtual ICollection<StoryMedium> StoryMedia { get; } = new List<StoryMedium>();
        
        public virtual User User { get; set; } = null!;

    }
}
