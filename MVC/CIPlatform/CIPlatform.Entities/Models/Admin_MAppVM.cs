using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.Models
{
    public class Admin_MAppVM
    {
        public long MissionApplicationId { get; set; }

        public long MissionId { get; set; }

        public long UserId { get; set; }

        public DateTime AppliedAt { get; set; }

        public string ApprovalStatus { get; set; } = null!;

        public virtual Mission Mission { get; set; } = null!;

        public virtual User User { get; set; } = null!;

    }
}
