using CIPlatform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repositories
{
   public interface IHomeRepository
    {

            IEnumerable<Story> GetStories();

            IEnumerable<User> GetUsers();
            IEnumerable<Mission> GetMission();
            
            public User UserDataforLogin(string emailpara, string passpara);
            IEnumerable<Story> GetTable1WithTable2Records();


            IEnumerable<MissionTheme> GetMissionThemes();

        ////IEnumerable<Mission> GetMissionWithMissionThemeRecords();
            //List<Mission> GetMissionWithMissionThemeRecords(string[]? themefilter, string[]? cityidarr, string[]? countryidarr);
            IEnumerable<Mission> GetSpecificMission(int id);

            IEnumerable<City> GetCityRecords();

            IEnumerable<Country> GetCountryRecords();

        List<Mission> GetMissionWithMissionThemeRecords(string[]? themefilter, string[]? cityidarr, string[]? countryidarr, string[]? skillidarr);

        IEnumerable<Story> GetSpecificStory(int id);

            public string GetLoginUser(int ids);

        public string GetUserEmail(int ids);


        IEnumerable<MissionSkill> GetSkillandMissionSkill();

        IEnumerable<GoalMission>GetGoalMissions();

        IEnumerable<GoalMission> GetTimeGoalBased(int id);

        IEnumerable<Skill> GetSkills();
        IEnumerable<UserSkill> GetUserSkillsList();
        IEnumerable<Skill> GetUserSkills(int skillsid);

        IEnumerable<MissionApplication> GetMissionAppList();
        IEnumerable<MissionApplication> Getappliedmissions(int ids);

        IEnumerable<GoalMission> GetAllGoalMissions(int missionid);

        IEnumerable<Timesheet> GetTimesheetsList();

        IEnumerable<Timesheet> GetTimesheetandMission(int selectedOptionId);

        public void deleteusers(int uid);

        IEnumerable<CmsPage> GetCMSData();
        public IEnumerable<MissionMedium> GetMissionMedia();
        public IEnumerable<MissionTheme> GetMissionThemess();

        public IEnumerable<MissionMedium> GetMissionMediaJoin(long missionid);

        public void deletemission(int missionid);



    }
    }



