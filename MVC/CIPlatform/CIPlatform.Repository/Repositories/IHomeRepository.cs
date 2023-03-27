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
            //IEnumerable<User> UserDataforLogin(string emailpara, string passpara);
            public User UserDataforLogin(string emailpara, string passpara);
            IEnumerable<Story> GetTable1WithTable2Records();


            //public MissionTheme GetMissionTheme();

            //public Mission GetMandMT();

            IEnumerable<MissionTheme> GetMissionThemes();

            IEnumerable<Mission> GetMissionWithMissionThemeRecords();


        }
    }



