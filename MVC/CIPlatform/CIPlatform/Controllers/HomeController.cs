using CIPlatform.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using CIPlatform.Models.ViewModels;
using CIPlatform.Entities.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CIPlatform.Repository.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;

namespace CIPlatform.Controllers
{
    public class HomeController : Controller
    {
        //CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
        private readonly ILogger<HomeController> _logger;
        public CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();

        public IHomeRepository _homeRepository;
        
        public HomeController(IHomeRepository loginRepository, ILogger<HomeController> logger)
        {
            _logger = logger;
            _homeRepository = loginRepository;
            
        }

       
        [HttpGet]
        public IActionResult Index()
        {
            LoginModel _loginModel = new LoginModel();
            return View(_loginModel);
        }
        [HttpPost]
        public IActionResult Index(LoginModel _loginModel)
        {

            //  var status = _ciplatformDbContext.Users.Where(u => u.Email == _loginModel.LoginId && u.Password == _loginModel.Password).FirstOrDefault();

            var status = _homeRepository.UserDataforLogin(_loginModel.LoginId, _loginModel.Password);

            if (status != null)
            {

                // string UserIDf = status.UserId.ToString();

                var a = status.UserId.ToString();
                string UserIDf = a;

                HttpContext.Session.SetString("userid", UserIDf);
                var abc = HttpContext.Session.GetString("userid");

                long abcd = Convert.ToInt64(abc);
             
                if (status != null)
                {
                    var loginfname = status.FirstName;
                    var loginlname = status.LastName;

                    TempData["fullname"] = loginfname + loginlname;

                    ViewBag.LoginStatus = 1;


                }


                return RedirectToAction("PlatformLandingPage", "Home", new { @Id = a });

            }
            else
            {
                ViewBag.LoginStatus = 0;
                //return RedirectToAction("Index", "Home");
               
            }

            return View(_loginModel);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userid");
            if (HttpContext.Session.GetString("userid") == null)
     
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("PlatformLandingPage", "Home");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LostPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LostPass(ForgotPasswordModel _forgotPasswordModel, PasswordReset passwordReset, User user)
        {
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();

            try
            {

                var generated_token = Guid.NewGuid().ToString();  //for token generation


                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress("tulsithakkar21@gmail.com", "CI PLATFORM");

                newMail.To.Add(user.Email);// declare the email subject

                newMail.Subject = "My First Email"; // use HTML for the email body

                newMail.IsBodyHtml = true;

                
                var lnkHref = Url.ActionLink("ResetPass", "Home", new { id = generated_token });

               

                newMail.Body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;

                // enable SSL for encryption across channels
                client.EnableSsl = true;
                // Port 465 for SSL communication
                client.Port = 587;
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                client.Credentials = new System.Net.NetworkCredential("tulsithakkar21@gmail.com", "gcfxdmdfccmpzjce");

                client.Send(newMail); // Send the constructed mail
                Console.WriteLine("Email Sent");

                try
                {
                    var passdata = new PasswordReset()
                    {
                        PassResetId = passwordReset.PassResetId,
                        Email = passwordReset.Email,
                        Token = generated_token
                    };
                    _ciplatformDbContext.PasswordResets.Add(passdata);
                    _ciplatformDbContext.SaveChanges();
                    ViewBag.Status = 1;
                }
                catch
                {
                    ViewBag.Status = 0;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error -" + ex);


            }


            return View();
        }


        [HttpGet]
        public IActionResult ResetPass()
        {
            return View();
        }



        [HttpPost]
        public IActionResult ResetPass(LoginModel modellogin, PasswordReset passwordReset)
        {


            var passwd = Request.Form["pass1"];
            var urlll = HttpContext.Request.GetDisplayUrl();
            // it will trim the url and only fetch the token from it

            string path = new Uri(urlll).LocalPath.Substring(16);
            Console.WriteLine(path);
            Console.WriteLine(passwd);
            
            var user1 = _ciplatformDbContext.PasswordResets.FirstOrDefault(x => (x.Token == path));
            if (user1 != null)
            {

                var emailtmp = user1.Email;
                var user2 = _ciplatformDbContext.Users.FirstOrDefault(u => (u.Email == emailtmp.ToLower()));

                if (user2 != null)
                {

                    user2.Password = passwd;
                    _ciplatformDbContext.Users.Update(user2);
                    _ciplatformDbContext.SaveChanges();

                    return RedirectToAction("Index", "Home");

                }


                else
                {
                    return RedirectToAction("ResetPass", "Home");

                }

            }


            return View();
        }
    





        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel _registrationModel)
        {
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();

            try
            {
                
                var flag = _ciplatformDbContext.Users.Where(a => a.Email == _registrationModel.Email);

                if (_registrationModel.Email != null && _registrationModel.PhoneNumber != null && _registrationModel.Password != null)
                {


                    if (flag.Count() == 0)
                    {
                        var userData = new User()
                        {
                            FirstName = _registrationModel.FirstName,
                            LastName = _registrationModel.LastName,
                            Email = _registrationModel.Email,
                            Password = _registrationModel.Password,
                            CityId = _registrationModel.CityId,
                            CountryId = _registrationModel.CountryId,
                            PhoneNumber = _registrationModel.PhoneNumber

                        };
                        _ciplatformDbContext.Users.Add(userData);
                        _ciplatformDbContext.SaveChanges();
                        ViewBag.Status = 1;

                        //return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ViewBag.Status = 0;
                        //return RedirectToAction("Registration", "Home");

                    }
                }
            }
            catch
            {
                ViewBag.Status = 0;
            }


            return View();
        }






        [HttpGet]
        public IActionResult PlatformLandingPage(string? subcats_id, string? filtercity, string? filtercountry,string? filterskill, string? searching, string? filter, string? sortOrder, int? page = 1, int? pageSize = 6, int id= 0)
        {
            //var goalmissionlist = _homeRepository.GetGoalMissions();
            //var timegoalbased = _homeRepository.GetTimeGoalBased(id);

            //ViewBag.timegoalbased = timegoalbased;
            
            // progress bar
            var progressBar = _ciplatformDbContext.GoalMissions.First(); // or any other way to get the progress bar value
            TempData.Clear();
            TempData["ProgressBarValue"] = progressBar.GoalValue;

            var progressData = _ciplatformDbContext.GoalMissions.Where(a => a.MissionId == id).ToList();
            ViewBag.progressData = progressData;



            int count = 30;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;


            var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
            ViewBag.favlist = favlist;
            
            
            //var Ratingdata = _ciplatformDbContext.MissionRatings.Where(a => a.UserId == ids).ToList();
            //ViewBag.Ratingdata = Ratingdata;

            var Ratingdata = _ciplatformDbContext.MissionRatings.ToList();
            ViewBag.Ratingdata = Ratingdata;

            var abc = HttpContext.Session.GetString("userid");
            if (abc == null)
            {
                return RedirectToAction("Login", "Login");
            }
         

            var missionxx = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();
            var outputsf = new List<Mission>();
            var citylists = _ciplatformDbContext.Cities.ToList();

            if (!string.IsNullOrEmpty(subcats_id) || !string.IsNullOrEmpty(filtercity) || !string.IsNullOrEmpty(filtercountry)|| !string.IsNullOrEmpty(filterskill))
            {
                string[] cityidarr = null;
                if (filtercity != null)
                {
                    cityidarr = filtercity.Split(",");
                }

                string[] themefilter = null;
                if (subcats_id != null)
                {
                    themefilter = subcats_id.Split(",");
                }
                string[] countryidarr = null;
                if (filtercountry != null)
                {
                    countryidarr = filtercountry.Split(",");
                }
                string[] skillidarr = null;
                if (filterskill != null)
                {
                    countryidarr = filterskill.Split(",");
                }
                outputsf = _homeRepository.GetMissionWithMissionThemeRecords(themefilter, cityidarr, countryidarr, skillidarr);
            }
            else
            {
                outputsf = missionxx;
            }

            //if (!string.IsNullOrEmpty(filtercity))
            //{

            // outputsf = _loginRepository.GetMissionWithMissionThemeRecords(cityidarr);
            //}
            //else
            //{
            // outputsf = missionxx;
            //}



            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.DateSortParamAsc = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LowestSeats = sortOrder == "LowSeats" ? "HighSeats" : "LowSeats";
            ViewBag.HighestSeats = sortOrder == "HighSeats" ? "LowSeats" : "HighSeats";

            switch (sortOrder)
            {

                case "Date":
                    outputsf = outputsf.OrderBy(a => a.StartDate).ToList();
                    break;
                case "date_desc":
                    outputsf = outputsf.OrderByDescending(a => a.StartDate).ToList();
                    break;
                case "LowSeats":
                    outputsf = outputsf.OrderBy(a => a.Availability).ToList();
                    break;
                case "HighSeats":
                    outputsf = outputsf.OrderByDescending(a => a.Availability).ToList();
                    break;
                default:
                    outputsf = outputsf.ToList();
                    break;
            }


            var currentDate = DateTime.Now;
            
            ViewBag.currentDate = currentDate;
            //var missionthemexx = _db.MissionThemes.Where(a => a.Title.Contains(filter) || filter == null).ToList();

            if (missionxx.Count == 0)
            {
                ViewBag.SearchStatus = 0;
            }


            var mission = _homeRepository.GetMission();
            var mt = _homeRepository.GetMissionThemes();

            @ViewData["page"] = page;
            ViewData["PageSize"] = pageSize;
            if (pageSize != null && page != null)
            {
                ViewData["TotalPages"] = (int)Math.Ceiling((decimal)outputsf.Count / (int)pageSize);
                ViewBag.outputsf = outputsf.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize).ToList();

            }

            var missionx = _ciplatformDbContext.Missions.ToList();
            ViewBag.Missions = missionx;


            var countryx = _ciplatformDbContext.Countries.ToList();
            ViewBag.Country = countryx;


            var cityx = _ciplatformDbContext.Cities.ToList();
            ViewBag.Cities = cityx;

            var themex = _ciplatformDbContext.MissionThemes.ToList();
            ViewBag.MissionThemes = themex;



            var skillx = _homeRepository.GetSkillandMissionSkill();
            ViewBag.MissionSkills = skillx;

            var totalmissions = outputsf.Count();
            ViewBag.Totalmissions = totalmissions;

            var missionapplication = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();
            ViewBag.missionapplication = missionapplication;

            return View();

        }
        
        public IActionResult _Grid(string? subcats_id, string? filtercity, string? filtercountry, string? filterskill, string? searching, string? filter, string? sortOrder, int? page = 1, int? pageSize = 6, int id = 0)
        {
            // progress bar

            //var goalmissionlist = _homeRepository.GetGoalMissions();
            //var timegoalbased = _homeRepository.GetTimeGoalBased(id);

            //ViewBag.goalmissionlist = goalmissionlist;



            var progressBar = _ciplatformDbContext.GoalMissions.First(); // or any other way to get the progress bar value
            TempData.Clear();
            TempData["ProgressBarValue"] = progressBar.GoalValue;

            var progressData = _ciplatformDbContext.GoalMissions.Where(a => a.MissionId == id).ToList();
            ViewBag.progressData = progressData;


            int count = 30;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;

            var currentDate = DateTime.Now;
          

            ViewBag.currentDate = currentDate;

            var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
            ViewBag.favlist = favlist;
            var Ratingdata = _ciplatformDbContext.MissionRatings.ToList();
            ViewBag.Ratingdata = Ratingdata;

            var abc = HttpContext.Session.GetString("userid");
            if (abc == null)
            {
                return RedirectToAction("Login", "Login");
            }


            var missionxx = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();
            var outputsf = new List<Mission>();
            var citylists = _ciplatformDbContext.Cities.ToList();

            //if (subcats_id.Count().ToString() == "2")
            //{
            //    return RedirectToAction("NoMissionFound", "Home");
            //}
            if (!string.IsNullOrEmpty(subcats_id) || !string.IsNullOrEmpty(filtercity) || !string.IsNullOrEmpty(filtercountry) || !string.IsNullOrEmpty(filterskill))
            {
                string[] cityidarr = null;
                if (filtercity != null)
                {
                    cityidarr = filtercity.Split(",");
                }

                string[] themefilter = null;
                if (subcats_id != null)
                {
                    themefilter = subcats_id.Split(",");
                }
                string[] countryidarr = null;
                if (filtercountry != null)
                {
                    countryidarr = filtercountry.Split(",");
                }
                string[] skillidarr = null;
                if (filterskill != null)
                {
                    countryidarr = filterskill.Split(",");
                }
                outputsf = _homeRepository.GetMissionWithMissionThemeRecords(themefilter, cityidarr, countryidarr, skillidarr);

               
            }

            else
            {
                outputsf = missionxx;
            }

            if (outputsf.Count() == 0)
            {
                ViewBag.SearchStatus = 0;
            }
            //if (!string.IsNullOrEmpty(filtercity))
            //{

            // outputsf = _loginRepository.GetMissionWithMissionThemeRecords(cityidarr);
            //}
            //else
            //{
            // outputsf = missionxx;
            //}



            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.DateSortParamAsc = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LowestSeats = sortOrder == "LowSeats" ? "HighSeats" : "LowSeats";
            ViewBag.HighestSeats = sortOrder == "HighSeats" ? "LowSeats" : "HighSeats";

            switch (sortOrder)
            {

                case "Date":
                    outputsf = outputsf.OrderBy(a => a.StartDate).ToList();
                    break;
                case "date_desc":
                    outputsf = outputsf.OrderByDescending(a => a.StartDate).ToList();
                    break;
                case "LowSeats":
                    outputsf = outputsf.OrderBy(a => a.Availability).ToList();
                    break;
                case "HighSeats":
                    outputsf = outputsf.OrderByDescending(a => a.Availability).ToList();
                    break;
                default:
                    outputsf = outputsf.ToList();
                    break;
            }


            //var missionthemexx = _db.MissionThemes.Where(a => a.Title.Contains(filter) || filter == null).ToList();

            if (missionxx.Count == 0)
            {
                ViewBag.SearchStatus = 0;
            }


            var mission = _homeRepository.GetMission();
            var mt = _homeRepository.GetMissionThemes();

            @ViewData["page"] = page;
            ViewData["PageSize"] = pageSize;
            if (pageSize != null && page != null)
            {
                ViewData["TotalPages"] = (int)Math.Ceiling((decimal)outputsf.Count / (int)pageSize);
                ViewBag.outputsf = outputsf.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize).ToList();

            }

            var missionx = _ciplatformDbContext.Missions.ToList();
            ViewBag.Missions = missionx;


            var countryx = _ciplatformDbContext.Countries.ToList();
            ViewBag.Country = countryx;


            var cityx = _ciplatformDbContext.Cities.ToList();
            ViewBag.Cities = cityx;

            var themex = _ciplatformDbContext.MissionThemes.ToList();
            ViewBag.MissionThemes = themex;



            var skillx = _homeRepository.GetSkillandMissionSkill();
            ViewBag.MissionSkills = skillx;


            var missionapplication = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids ).ToList();
            ViewBag.missionapplication = missionapplication;

            return View();


        }




        public IActionResult _List(string? subcats_id, string? filtercity, string? filtercountry, string? filterskill, string? searching, string? filter, string? sortOrder, int? page = 1, int? pageSize = 6)
        {
         


            int count = 30;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;

            var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
            ViewBag.favlist = favlist;
            var Ratingdata = _ciplatformDbContext.MissionRatings.ToList();
            ViewBag.Ratingdata = Ratingdata;

            var abc = HttpContext.Session.GetString("userid");
            if (abc == null)
            {
                return RedirectToAction("Login", "Login");
            }


            var missionxx = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();
            var outputsf = new List<Mission>();
            var citylists = _ciplatformDbContext.Cities.ToList();

            if (!string.IsNullOrEmpty(subcats_id) || !string.IsNullOrEmpty(filtercity) || !string.IsNullOrEmpty(filtercountry) || !string.IsNullOrEmpty(filterskill))
            {
                string[] cityidarr = null;
                if (filtercity != null)
                {
                    cityidarr = filtercity.Split(",");
                }

                string[] themefilter = null;
                if (subcats_id != null)
                {
                    themefilter = subcats_id.Split(",");
                }
                string[] countryidarr = null;
                if (filtercountry != null)
                {
                    countryidarr = filtercountry.Split(",");
                }
                string[] skillidarr = null;
                if (filterskill != null)
                {
                    skillidarr = filterskill.Split(",");
                }
                outputsf = _homeRepository.GetMissionWithMissionThemeRecords(themefilter, cityidarr, countryidarr, skillidarr);
            }
            else
            {
                outputsf = missionxx;
            }

            //if (!string.IsNullOrEmpty(filtercity))
            //{

            // outputsf = _loginRepository.GetMissionWithMissionThemeRecords(cityidarr);
            //}
            //else
            //{
            // outputsf = missionxx;
            //}

            if (outputsf.Count() == 0)
            {
                ViewBag.SearchStatus = 0;
            }


            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.DateSortParamAsc = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LowestSeats = sortOrder == "LowSeats" ? "HighSeats" : "LowSeats";
            ViewBag.HighestSeats = sortOrder == "HighSeats" ? "LowSeats" : "HighSeats";

            switch (sortOrder)
            {

                case "Date":
                    outputsf = outputsf.OrderBy(a => a.StartDate).ToList();
                    break;
                case "date_desc":
                    outputsf = outputsf.OrderByDescending(a => a.StartDate).ToList();
                    break;
                case "LowSeats":
                    outputsf = outputsf.OrderBy(a => a.Availability).ToList();
                    break;
                case "HighSeats":
                    outputsf = outputsf.OrderByDescending(a => a.Availability).ToList();
                    break;
                default:
                    outputsf = outputsf.ToList();
                    break;
            }


            //var missionthemexx = _db.MissionThemes.Where(a => a.Title.Contains(filter) || filter == null).ToList();

            if (missionxx.Count == 0)
            {
                ViewBag.SearchStatus = 0;
            }


            var mission = _homeRepository.GetMission();
            var mt = _homeRepository.GetMissionThemes();

            @ViewData["page"] = page;
            ViewData["PageSize"] = pageSize;
            if (pageSize != null && page != null)
            {
                ViewData["TotalPages"] = (int)Math.Ceiling((decimal)outputsf.Count / (int)pageSize);
                ViewBag.outputsf = outputsf.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize).ToList();

            }

            var missionx = _ciplatformDbContext.Missions.ToList();
            ViewBag.Missions = missionx;


            var countryx = _ciplatformDbContext.Countries.ToList();
            ViewBag.Country = countryx;


            var cityx = _ciplatformDbContext.Cities.ToList();
            ViewBag.Cities = cityx;

            var themex = _ciplatformDbContext.MissionThemes.ToList();
            ViewBag.MissionThemes = themex;



            //var skillx = _ciplatformDbContext.MissionSkills.ToList();
            //ViewBag.MissionSkills = skillx;

            var skillx = _homeRepository.GetSkillandMissionSkill();
            ViewBag.MissionSkills = skillx;



            return View();


        }





        [HttpPost]
        public async Task<IActionResult> AddFav(long missionId)
        {

            var id = HttpContext.Session.GetString("userid");
            FavoriteMission favoriteMission = await _ciplatformDbContext.FavoriteMissions.FirstOrDefaultAsync(fm => fm.UserId.ToString() == id && fm.MissionId == missionId);

            ViewBag.ids = Convert.ToInt16(id);
            if (favoriteMission != null)
            {
                // Remove the favorite mission from the database if it already exists
                _ciplatformDbContext.FavoriteMissions.Remove(favoriteMission);
                await _ciplatformDbContext.SaveChangesAsync();
                var favlist = _ciplatformDbContext.FavoriteMissions.ToList();
                ViewBag.favlist = favlist;
                ViewBag.isLiked = false;
                return RedirectToAction("PlatformLandingPage", "Home");

            }
            else
            {
                // Add the favorite mission to the database if it does not exist
                FavoriteMission newFavoriteMission = new FavoriteMission();
                newFavoriteMission.MissionId = missionId;
                newFavoriteMission.UserId = Convert.ToInt32(id);
                await _ciplatformDbContext.FavoriteMissions.AddAsync(newFavoriteMission);
                await _ciplatformDbContext.SaveChangesAsync();
                var favlist = _ciplatformDbContext.FavoriteMissions.ToList();
                ViewBag.favlist = favlist;
                ViewBag.isLiked = false;
                return RedirectToAction("PlatformLandingPage", "Home");

            }

        }



        [HttpPost]
        public IActionResult Rate(int stars, long missionId)
        {

            var b = missionId;
            var userId = HttpContext.Session.GetString("userid");
        var userids = Convert.ToInt32(userId);
            ViewBag.userids = userids;
            //var userexist = _ciplatformDbContext.MissionRatings.FirstOrDefault(x => x.MissionId == missionId && x.UserId == Convert.ToInt32(userId));

            // (userexist == null)
            //{
            var isRated = _ciplatformDbContext.MissionRatings.Where(r => r.UserId == userids && r.MissionId == missionId);

            if (isRated == null)
            {

                HttpContext.Session.SetInt32("missionId", Convert.ToInt32(b));

                var msnid = HttpContext.Session.GetInt32("missionId");

                var rating = new MissionRating
                {

                    Rating = stars.ToString(),
                    UserId = Convert.ToInt32(userId),
                    MissionId = missionId


                };

                _ciplatformDbContext.MissionRatings.Add(rating);
               
                _ciplatformDbContext.SaveChanges();

            }
            else {
            
                var ratinguserdata = _ciplatformDbContext.MissionRatings.Where(y => y.MissionId == missionId && y.UserId== userids).ToList();

                var query = from r in ratinguserdata select r;
                foreach (MissionRating r in query) { 
                   r.Rating = stars.ToString();
                }

                _ciplatformDbContext.SaveChanges();
            }
           
           

          
            //}
            return RedirectToAction("PlatformLandingPage" , "Home");
        }

       


        [HttpPost]
        public IActionResult Recommendedto(string checkbox_value, int MissionId)
        {

            
            string[] usersList = { "," };
            int count = 30;
            string[] users = checkbox_value.Split(",", count, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in users)
            {
                var k = s;
                Console.WriteLine(k);



                try
                {
                    MailMessage newMail = new MailMessage();
                    SmtpClient client = new SmtpClient("smtp.gmail.com");
                    newMail.From = new MailAddress("tulsithakkar21@gmail.com", "CI PLATFORM");
                    newMail.To.Add(s);// declare the email subject
                    newMail.Subject = "Recommended Mission";
                    newMail.IsBodyHtml = true;
                    var lnkHref = Url.ActionLink("VolunteeringMission", "Home", new { id = MissionId });
                    newMail.Body = "<b>Hey, Click the below link to find mission as per your skills...</b><br/>" + lnkHref;
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("tulsithakkar21@gmail.com", "gcfxdmdfccmpzjce");
                    client.Send(newMail); // Send the constructed mail
                    Console.WriteLine("Email Sent");



                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error -" + ex);
                }

            }

        


            return View();

        }








        public IActionResult VolunteeringMission(int id, string commenttext, int MissionId, string searching, int? page = 1, int? pageSize = 9, int missionidforapply=0)
        {

            // progress bar
            var progressBar = _ciplatformDbContext.GoalMissions.First(); // or any other way to get the progress bar value
            TempData.Clear();
            TempData["ProgressBarValue"] = progressBar.GoalValue;

            var progressData = _ciplatformDbContext.GoalMissions.Where(a => a.MissionId == id).ToList();
            ViewBag.progressData = progressData;


            var userslist = _ciplatformDbContext.Users.ToList();
            ViewBag.userslist = userslist;
            var currentDate = DateTime.Now;

            ViewBag.currentDate = currentDate;


            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;

            var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
            ViewBag.favlist = favlist;
            
            var b = id;

           
            if (id == 0)
            {
                id = MissionId;
            }




            var specificmission = _homeRepository.GetSpecificMission(id);
            var citylist = _homeRepository.GetCityRecords();
            var countrylist = _homeRepository.GetCountryRecords();
            var missionthemelist = _homeRepository.GetMissionThemes();





            //var missionthemelist = _ciplatformDbContext.MissionThemes.ToList();
            //var citylist = _ciplatformDbContext.Cities.ToList();
            //var countrylist = _ciplatformDbContext.Countries.ToList();
            //var specificmission = _ciplatformDbContext.Missions.Where(a => a.MissionId == b).ToList();

            // var documentlist = _ciplatformDbContext.MissionDocuments.Where(a => a.MissionId == b).ToList();


            //if (documentlist.Count != 0)
            //{
            //    var result = from m in specificmission
            //                 join mt in missionthemelist on m.ThemeId equals mt.MissionThemeId
            //                 where m.ThemeId == mt.MissionThemeId
            //                 join cty in citylist on m.CityId equals cty.CityId
            //                 where cty.CityId == m.CityId
            //                 join docs in documentlist on m.MissionId equals docs.MissionId
            //                 where m.MissionId == docs.MissionId
            //                 select new
            //                 {

            //                     m,
            //                     mt.Title,
            //                     mt.MissionThemeId,
            //                     cty.CityId,
            //                     cty.Name,
            //                     docs.DocumentName,
            //                     docs.DocumentPath

            //                 };
            //    ViewBag.Result = result;
            //}

            //else
            //{
            //    var result = from m in specificmission
            //                 join mt in missionthemelist on m.ThemeId equals mt.MissionThemeId
            //                 where m.ThemeId == mt.MissionThemeId
            //                 join cty in citylist on m.CityId equals cty.CityId
            //                 where cty.CityId == m.CityId

            //                 select new
            //                 {

            //                     m,
            //                     mt.Title,
            //                     mt.MissionThemeId,
            //                     cty.CityId,
            //                     cty.Name,


            //                 };
            //    ViewBag.Result = result;
            //}

            ViewBag.specificmission = specificmission;



            var missionthemexx = _ciplatformDbContext.MissionThemes.ToList();
            var cityxx = _ciplatformDbContext.Cities.ToList();

            var missionx = _ciplatformDbContext.Missions.ToList();
            ViewBag.Missions = missionx;


            var countryx = _ciplatformDbContext.Countries.ToList();
            ViewBag.Country = countryx;

            var cityx = _ciplatformDbContext.Cities.ToList();
            ViewBag.Cities = cityx;

            var themex = _ciplatformDbContext.MissionThemes.ToList();
            ViewBag.MissionThemes = themex;



            var skillx = _ciplatformDbContext.MissionSkills.ToList();
            ViewBag.MissionSkills = skillx;





            var finalcity = (from m in specificmission select m.CityId).FirstOrDefault();
            var finalcntry = (from m in specificmission select m.CountryId).FirstOrDefault();
            var finaltheme = (from m in specificmission select m.ThemeId).FirstOrDefault();

           

            var missionxx = _ciplatformDbContext.Missions.Where(a => a.CityId == finalcity && a.MissionId != b).ToList();

            if (missionxx.Count == 0)
            {
                missionxx = _ciplatformDbContext.Missions.Where(a => a.CountryId == finalcntry && a.MissionId != b).ToList();
                if (missionxx.Count == 0)
                {
                    missionxx = _ciplatformDbContext.Missions.Where(a => a.ThemeId == finaltheme && a.MissionId != b).ToList();

                }

            }





            var result2 = from m in missionxx
                          join mt in missionthemexx on m.ThemeId equals mt.MissionThemeId
                          where m.ThemeId == mt.MissionThemeId
                          join cty in cityxx on m.CityId equals cty.CityId
                          where cty.CityId == m.CityId
                          select new
                          {

                              m,
                              mt.Title,
                              mt.MissionThemeId,
                              cty.CityId,
                              cty.Name
                          };
            ViewBag.Result2 = result2;




            //for comments


            var cmtlist = _ciplatformDbContext.Comments.Where(a => a.MissionId == id).ToList();
            var usrlist = _ciplatformDbContext.Users.ToList();
            string processedData = commenttext;

            ViewBag.ids = Convert.ToInt32(ids);

            var comnt = from c in cmtlist
                        join u in usrlist on c.UserId equals u.UserId
                        where c.UserId == u.UserId

                        select new
                        {
                            c,
                            u
                        };
            ViewBag.comnt = comnt;
            if (commenttext != null)
            {

                var cmnt = new Comment
                {
                    CommentText = commenttext,
                    UserId = ids,
                    MissionId = id
                };

                _ciplatformDbContext.Comments.Add(cmnt);

                _ciplatformDbContext.SaveChanges();

            }


            // for search

            var search = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();

            if (search.Count == 0)
            {
                ViewBag.SearchStatus = 0;
            }


            // for skills

            var skillsList = _ciplatformDbContext.Skills.ToList();
            var missionskillList = _ciplatformDbContext.MissionSkills.Where(a => a.MissionId == id).ToList();


            var skilldata = from s in skillsList
                            join ms in missionskillList on s.SkillId equals ms.SkillId
                            where ms.SkillId == s.SkillId
                            select new
                            {
                                s,
                                ms
                            };

            ViewBag.skilldata = skilldata;


            // for recent vol

            var userList = _homeRepository.GetUsers();
            var missionapply = _ciplatformDbContext.MissionApplications.Where(a=>a.MissionId == id && a.UserId == ids).ToList();

            var finalresult = from u in userList join m in missionapply on u.UserId equals m.UserId where u.UserId == m.UserId select new { u.FirstName, m };
            //ViewBag.userlist = userList;
            ViewBag.finalresult = finalresult;
            @ViewData["page"] = page;
            ViewData["PageSize"] = pageSize;
            if (pageSize != null && page != null)
            {
                ViewData["TotalPages"] = (int)Math.Ceiling((decimal)missionxx.Count / (int)pageSize);
                ViewBag.userlist = userList.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize).ToList();

            }



            //ratings

            var missionapplication = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids && a.MissionId == id).ToList();

            ViewBag.missionapplication = missionapplication;

            var Ratingdata = _ciplatformDbContext.MissionRatings.Where(a => a.UserId == ids).ToList();
            ViewBag.Ratingdata = Ratingdata;


            // applied mission


            if (missionidforapply != 0)
            {
                var appliedmission = new MissionApplication
                {

                    UserId = ids,
                    MissionId = missionidforapply,
                    CreatedAt = DateTime.Now,
                    AppliedAt = DateTime.Now,



                };
                _ciplatformDbContext.MissionApplications.Add(appliedmission);
                _ciplatformDbContext.SaveChanges();

            }

            return View();


           
        }


        public IActionResult StoryListing()
        {
            var listofstories = _homeRepository.GetStories();
            var user = _homeRepository.GetUsers();
            var mis = _homeRepository.GetMission();
            var res = _homeRepository.GetTable1WithTable2Records();

            ViewBag.listofstories = res;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;


            return View(listofstories);


        }


        //public IActionResult StoryListing(String searching)
        //{

        //    var storyList = _ciplatformDbContext.Stories.ToList();
        //    var usrlist = _ciplatformDbContext.Users.ToList();
        //    var missionList = _ciplatformDbContext.Missions.ToList();
        //    var missionThemeList = _ciplatformDbContext.MissionThemes.ToList();

        //    var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        //    ViewBag.ids = Convert.ToInt32(ids);





        //    var stories = from s in storyList
        //                join u in usrlist on s.UserId equals u.UserId
        //                where s.UserId == u.UserId
        //                join m in missionList on s.MissionId equals m.MissionId
        //                where s.MissionId == m.MissionId
        //                join mt in missionThemeList on m.ThemeId equals mt.MissionThemeId
        //                where m.ThemeId == mt.MissionThemeId

        //                  select new
        //                {
        //                    s,
        //                    u,
        //                    m,
        //                    mt
        //                };

        //    ViewBag.stories = stories;


        //    // for search

        //    var search = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();

        //    if (search.Count == 0)
        //    {
        //        ViewBag.SearchStatus = 0;
        //    }






        //    return View();
        //}




        public IActionResult ShareYourStory(string videourl,int missiondd, string storyTitle, string abcd, int id, string desc, DateTime pubDate, string newArray, int selectedOptionId = 0)
        {


            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);

            var appliedmission = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();

            ViewBag.appliedmission = appliedmission;
            var missionidfinal = _ciplatformDbContext.Missions.Where(a => a.Title.Contains(abcd)).ToList();
            long missinids = 0;
            missionidfinal.ForEach(mission => missinids = mission.MissionId);
            var storylist = _ciplatformDbContext.Stories.Where(a => a.Status == "Draft");
            ViewBag.medias = _ciplatformDbContext.MissionMedia.ToList();
            if (selectedOptionId != 0)
            {
                var draftedstory = _ciplatformDbContext.Stories.Where(a => a.MissionId == selectedOptionId && a.UserId == ids).ToList();



                var query = from r in draftedstory select r;
                foreach (Story r in query)
                {
                    r.Status = "Save";
                }

                _ciplatformDbContext.SaveChanges();
            }



            var missionList = _ciplatformDbContext.Missions.ToList();

            if (storyTitle != null)
            {
                var isstoryexist = _ciplatformDbContext.Stories.Where(a => a.MissionId == missiondd && a.UserId == ids).ToList();
                if (isstoryexist.Count == 0)
                {
                    var story = new Story
                    {

                        Title = storyTitle,
                        Description = desc,
                        PublishedAt = pubDate,
                        UserId = ids,
                        MissionId = missiondd
                    };

                    _ciplatformDbContext.Stories.Add(story);

                    _ciplatformDbContext.SaveChanges();
                }

                else
                {
                    var query = from s in isstoryexist select s;
                    foreach (Story s in query)
                    {
                        s.Title = storyTitle;
                        s.Description = desc;
                        s.PublishedAt = pubDate;
                        s.VidUrl = videourl;
                        s.UserId = ids;
                        s.MissionId = missiondd;
                    }

                    _ciplatformDbContext.SaveChanges();


                }
            }



            var finallist = from a in appliedmission
                            join m in missionList on a.MissionId equals m.MissionId
                            where a.MissionId == m.MissionId
                            //join s in storylist on a.MissionId equals s.MissionId

                            select new
                            {
                                a,
                                m
                            };



            ViewBag.finallist = finallist;

            if (newArray != null)
            {

                string[] imgdata = newArray.Split(",", StringSplitOptions.RemoveEmptyEntries);


                // Save the images to disk or database
                foreach (var image in imgdata)
                {

                    var fileName = Path.GetFileName(image);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "~/NewFolder/", fileName);

                    string substring = filePath.Substring(63);
                    string imgname = filePath.Substring(74);

                    ViewBag.substring = substring;
                    ViewBag.isempty = 1;


                    var imgdb = new MissionMedium
                    {
                        MissionId = 5,
                        MediaName = imgname,

                        MediaType = "png",
                        MediaPath = substring

                    };

                    _ciplatformDbContext.MissionMedia.Add(imgdb);
                    _ciplatformDbContext.SaveChanges();

                }

            }


            var abc = HttpContext.Session.GetString("userid");
            if (abc == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }










        //public IActionResult ShareYourStory(string storyTitle,string appliedMission , int id, string desc, DateTime pubDate, string vid, int selectedOptionId)
        //{
        //    var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        //    ViewBag.ids = Convert.ToInt32(ids);
        //    var fullname = _homeRepository.GetLoginUser(ids);
        //    ViewBag.fullname = fullname;

        //    var appliedmission = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();

        //    ViewBag.appliedmission = appliedmission;
        //    var missionidfinal = _ciplatformDbContext.Missions.Where(a=>a.Title.Contains(appliedMission)).ToList();
        //    long missinids = 0;
        //    missionidfinal.ForEach(mission => missinids = mission.MissionId);





        //    var missionList = _ciplatformDbContext.Missions.ToList();

        //    if (storyTitle != null)
        //    {

        //        var story = new Story
        //        {

        //            Title = storyTitle,
        //            Description = desc,
        //            PublishedAt = pubDate,
        //            VidUrl = vid,
        //            UserId = ids,
        //            MissionId = missinids
        //        };

        //        _ciplatformDbContext.Stories.Add(story);

        //        _ciplatformDbContext.SaveChanges();

        //    }



        //    var finallist = from a in appliedmission
        //                    join m in missionList on a.MissionId equals m.MissionId
        //                    where a.MissionId == m.MissionId
        //                    select new
        //                    {
        //                        a,
        //                        m
        //                    };



        //    ViewBag.finallist = finallist;



        //    //Edit






        //    return View();
        //}


        public IActionResult EditStory(int selectedOptionId, string appliedMission)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);


            var appliedmission = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();

            
            var missionidfinal = _ciplatformDbContext.Missions.Where(a => a.Title.Contains(appliedMission)).ToList();
            long missinids = 0;
            missionidfinal.ForEach(mission => missinids = mission.MissionId);

            var missionList = _ciplatformDbContext.Missions.ToList();

            var finallist = from a in appliedmission
                            join m in missionList on a.MissionId equals m.MissionId
                            where a.MissionId == m.MissionId
                            select new
                            {
                                a,
                                m
                            };



            

            var data = _ciplatformDbContext.Stories.Where(a => a.MissionId == selectedOptionId && a.UserId == ids).ToList().FirstOrDefault();

            
            if (data != null)
            {
                var fetchStoryDetails = new Story
                {

                    Title = data.Title,
                    Description = data.Description,
                    PublishedAt = data.PublishedAt,
                    VidUrl = data.VidUrl,
                    UserId = ids,
                    MissionId = missinids
                };

                //ViewBag.fetchStoryDetails = fetchStoryDetails;
                //ViewBag.checkempty = 1;
                return Json(fetchStoryDetails);
            }
            else
            {

                //ViewBag.checkempty = 0;
                return Json(null);
            }

         

            
        }

        public IActionResult SaveStory(string storyTitle, string appliedMission, int id, string desc, DateTime pubDate, string vid)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var storydata = _ciplatformDbContext.Stories.Where(y => y.MissionId == 2 && y.UserId == ids).ToList();

            var query = from s in storydata select s;
            foreach (Story s in query)
            {
                s.Title = storyTitle;
                s.Description = desc;
                s.PublishedAt = pubDate;
                s.VidUrl = vid;
                s.UserId = ids;
                s.MissionId = 2;
            }

            _ciplatformDbContext.SaveChanges();
            return RedirectToAction("ShareYourStory", "Home");
        }


       




        // upload imgssss







        public IActionResult StoryDetails(int id)
        {

            var specificStory = _homeRepository.GetSpecificStory(id);
            var mis = _homeRepository.GetMission();
            var user = _homeRepository.GetUsers();
            ViewBag.specificStory = specificStory;


            //recom to co-w
            
            ViewBag.user = user;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;

            

            return View();
        }



        public IActionResult EditProfile()
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);


            var data = _ciplatformDbContext.Users.Where(a => a.UserId == ids).ToList();


            if (data != null)
            {
                foreach (var a in data)
                {

                    ViewBag.FirstName = a.FirstName;
                    ViewBag.LastName = a.LastName;
                    ViewBag.EmployeeId = Convert.ToInt32(a.EmployeeId);
                    ViewBag.Title = a.Title;
                    ViewBag.Department = a.Department;
                    ViewBag.ProfileText = a.ProfileText;
                    ViewBag.WhyIVolunteer = a.WhyIVolunteer;
                    ViewBag.CityId = Convert.ToInt32(a.CityId);
                    ViewBag.LinkedInUrl = a.LinkedInUrl;

                }


                return View();


            }
            else
            {
                return View();

            }

        }



        public IActionResult SaveUserData(string firstname, string lastname, int id, string empId, string title, string dept, string profile, string whyI, int cityId, string linkedInurl)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);

            var userData = _ciplatformDbContext.Users.Where(y => y.UserId == ids).ToList();

            var query = from u in userData select u;

            foreach (User u in query)
            {
                u.FirstName = firstname;
                u.LastName = lastname;
                u.EmployeeId = empId;
                u.Title = title;
                u.Department = dept;
                u.ProfileText = profile;
                u.WhyIVolunteer = whyI;
                u.CityId = cityId;
                u.LinkedInUrl = linkedInurl;
                u.UserId = ids;

            }

            _ciplatformDbContext.SaveChanges();
            return RedirectToAction("EditProfile", "Home");
        }

        
        public IActionResult ChangeUserPass(string input1)
        {
            
            ChangePassUserModel _ch = new ChangePassUserModel();
            //var passwd = Request.Form["newpass"];

            //var passwd = _changePassUserModel.NewPassword;

            //var passwd = input1;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);



            var user = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));

            if (user != null)
            {

                user.Password = input1;
                _ciplatformDbContext.Users.Update(user);
                _ciplatformDbContext.SaveChanges();


                return View("Index");
               

            }

            else
            {
                return RedirectToAction("EditProfile", "Home");

            }

            
        }

        public IActionResult NoMissionFound()
        {
            return View();
        }
       



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}