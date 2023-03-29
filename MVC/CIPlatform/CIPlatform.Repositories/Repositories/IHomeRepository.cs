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
            List<Mission> GetMissionWithMissionThemeRecords(string[]? themefilter, string[]? cityidarr, string[]? countryidarr);
            IEnumerable<Mission> GetSpecificMission(int id);

            IEnumerable<City> GetCityRecords();

            IEnumerable<Country> GetCountryRecords();

            IEnumerable<Story> GetSpecificStory(int id);
    }
    }



