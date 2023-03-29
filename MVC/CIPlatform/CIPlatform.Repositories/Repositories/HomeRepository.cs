using CIPlatform.Entities.Models;
//using CIPlatform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        public readonly CiplatformDbContext _db;
        public HomeRepository(CiplatformDbContext db)
        {
            _db = db;
        }


        public IEnumerable<Story> GetStories()
        {

            return _db.Stories.ToList();
        }



        public IEnumerable<User> GetUsers()
        {

            return _db.Users.ToList();

        }

        public IEnumerable<Mission> GetMission()
        {

            return _db.Missions.ToList();


        }



        public IEnumerable<Story> GetTable1WithTable2Records()
        {
            return _db.Stories
            .Include(t1 => t1.User)
            .Include(t2 => t2.Mission)

            .ToList();
        }



        //var user = _db.Users.FirstOrDefault(u => (u.Email == model.Email.ToLower() && u.Password == model.Password));

        public User UserDataforLogin(string emailpara, string passpara)
        {

            var existinguser = _db.Users.FirstOrDefault(u => u.Email == emailpara.ToLower() && u.Password == passpara);

            return existinguser;


        }

        public IEnumerable<MissionTheme> GetMissionThemes()
        {

            return _db.MissionThemes.ToList();

        }
        List<Mission> FinalMissionList = new List<Mission>();
        public List<Mission> GetMissionWithMissionThemeRecords(string[]? themefilter, string[]? cityidarr, string[]? countryidarr)

        {
            if (themefilter != null)
            {
                foreach (var theme in themefilter)
                {
                    var mission = _db.Missions
                    .Where(a => a.ThemeId == Convert.ToInt64(theme) || theme == null)

                    //where m.ThemeId == mt.MissionThemeId
                    .ToList();
                    FinalMissionList.AddRange(mission);

                }

            }


            if (cityidarr != null)
            {
                foreach (var city in cityidarr)
                {
                    var mission = _db.Missions
                    .Where(a => a.CityId == Convert.ToInt64(city) || city == null)

                    //where m.ThemeId == mt.MissionThemeId
                    .ToList();
                    FinalMissionList.AddRange(mission);

                }

            }

            if (countryidarr != null)
            {
                foreach (var country in countryidarr)
                {
                    var mission = _db.Missions
                    .Where(a => a.CountryId == Convert.ToInt64(country) || country == null)

                    //where m.ThemeId == mt.MissionThemeId
                    .ToList();
                    FinalMissionList.AddRange(mission);

                }

            }

            return FinalMissionList;
        }
        //public IEnumerable<Mission> GetMissionWithMissionThemeRecords()

        //{
        //    return _db.Missions


        //    //.Where(a => a.ThemeId == MissionTheme.MissionThemeId)
        //    .Include(t2 => t2.Theme)
        //    //where m.ThemeId == mt.MissionThemeId
        //    .ToList();
        //}


        public IEnumerable<City> GetCityRecords()
        {

            return _db.Cities.ToList();
        }

        public IEnumerable<Country> GetCountryRecords()
        {
            return _db.Countries.ToList();
        }

        public IEnumerable<Mission> GetSpecificMission(int id)
        {
            var b = id;
            //var specificmission = _db.Missions.Where(a => a.MissionId == b).ToList();
            
            //return specificmission;

            return _db.Missions.Where(a => a.MissionId == b)
                .Include(t1 => t1.City)
                .Include(t2 => t2.Country)
                .Include(t3 => t3.Theme) .ToList();
        }








    }


}