using CIPlatform.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CIPlatform.Models.ViewModels
{
    public class TimesheetVM
    
        {
            [ValidateNever]
            public long TimeSheetId { get; set; }

            public Mission? TimeMission { get; set; }

            public Mission? GoalMission { get; set; }

            public DateTime VolunteerDate { get; set; }
            public int VolunteerHrs { get; set; }

            public int VolunteerMins { get; set; }

            public int? GoalAction { get; set; } = 0;

            public string? Notes { get; set; }

            public long MissionId { get; set; }

            public string MissionName { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            [ValidateNever]
            public List<Mission>? GoalMissionsList { get; set; }
            [ValidateNever]
            public List<Mission>? TimeMissionsList { get; set; }

            [ValidateNever]
            public List<Timesheet>? GoalTs { get; set; }
            [ValidateNever]
            public List<Timesheet>? TimeTs { get; set; }
        }
    }



