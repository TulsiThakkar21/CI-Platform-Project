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
using System.Globalization;
using System.Web;

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


            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;

            return View(_loginModel);


        }
        [HttpPost]
        public IActionResult Index(LoginModel _loginModel)
        {

            //  var status = _ciplatformDbContext.Users.Where(u => u.Email == _loginModel.LoginId && u.Password == _loginModel.Password).FirstOrDefault();

            var status = _homeRepository.UserDataforLogin(_loginModel.LoginId, _loginModel.Password);

            var admin = _ciplatformDbContext.Admins.FirstOrDefault(a => a.Email == _loginModel.LoginId && a.Password == _loginModel.Password);

            if (admin == null)
            {
                var user = _ciplatformDbContext.Users.FirstOrDefault(u => (u.Email == _loginModel.LoginId.ToLower() && u.Password == _loginModel.Password));



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

                    return RedirectToAction("EditProfile", "Home", new { @Id = a });
                }

                else
                {
                    ViewBag.LoginStatus = 0;
                    //return RedirectToAction("Index", "Home");

                }
            }


            else
            {
                ViewBag.LoginStatus = 1;
                string UserIDf = admin.AdminId.ToString();


                HttpContext.Session.SetString("userid", UserIDf);

                var abc = HttpContext.Session.GetString("userid");

                var credentials = _loginModel.LoginId + _loginModel.Password;

                HttpContext.Session.SetString("credentials", credentials);


                long abcd = Convert.ToInt64(abc);
                var loginuser = _ciplatformDbContext.Admins.FirstOrDefault(x => (admin.AdminId == abcd));
                if (loginuser != null)
                {
                    var loginfname = loginuser.FirstName;
                    var loginlname = loginuser.LastName;

                    TempData["fullname"] = loginfname + loginlname;



                    return RedirectToAction("Admin_user", "Home", new { @Id = admin.AdminId });


                }
                else
                {
                    return View();
                }



            }



            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;


            return View(_loginModel);
        }





        public ActionResult Logout()
        {

            HttpContext.Session.Remove("credentials");
            HttpContext.Session.Remove("userid");
            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Registration", "Home");
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LostPass()
        {
            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;

            return View();
        }

        [HttpPost]
        public IActionResult LostPass(ForgotPasswordModel _forgotPasswordModel, PasswordReset passwordReset, User user)
        {
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;
            try
            {

                var generated_token = Guid.NewGuid().ToString();  //for token generation


                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress("tulsithakkar21@gmail.com", "CI PLATFORM");

                newMail.To.Add(user.Email);// declare the email subject

                newMail.Subject = "Reset Password"; // use HTML for the email body

                newMail.IsBodyHtml = true;


                //var lnkHref = Url.ActionLink("ResetPass", "Home", new { id = generated_token });
                DateTime expirationTime = DateTime.UtcNow.AddHours(2);
                var lnkHref = Url.ActionLink("ResetPass", "Home", new { id = HttpUtility.UrlEncode(generated_token), expiration = expirationTime.ToString("O") });


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
            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;

            return View();
        }



        //[HttpPost]
        //public IActionResult ResetPass(LoginModel modellogin, PasswordReset passwordReset)
        //{

        //    var bannerdata = _ciplatformDbContext.Banners.ToList();
        //    ViewBag.bannerdata = bannerdata;

        //    var passwd = Request.Form["Password"];
        //    var urlll = HttpContext.Request.GetDisplayUrl();
        //    // it will trim the url and only fetch the token from it

        //    string paths = urlll.Split("/")[5];

        //    string newpath = paths.Split("?")[0];



        //    string time = urlll.Split("=")[1];
        //    DateTime expirationTime = DateTime.ParseExact(HttpUtility.UrlDecode(time), "O", null);

        //    Console.WriteLine(paths);
        //    Console.WriteLine(passwd);

        //    var user1 = _ciplatformDbContext.PasswordResets.FirstOrDefault(x => (x.Token == newpath));
        //    if (user1 != null && expirationTime > DateTime.Now)
        //    {

        //        var emailtmp = user1.Email;
        //        var user2 = _ciplatformDbContext.Users.FirstOrDefault(u => (u.Email == emailtmp.ToLower()));

        //        if (user2 != null)
        //        {

        //            user2.Password = passwd;
        //            _ciplatformDbContext.Users.Update(user2);
        //            _ciplatformDbContext.SaveChanges();

        //            return RedirectToAction("Index", "Home");

        //        }


        //        else
        //        {
        //            return RedirectToAction("ResetPass", "Home");

        //        }

        //    }

        //    else
        //    {
        //        TempData["DataSavedMessage"] = "Your reset password link has been expired, please try again.";

        //        return View();
        //    }


        //    return View();
        //}

        [HttpPost]
        public IActionResult ResetPass(LoginModel modellogin, PasswordReset passwordReset)
        {


            var passwd = Request.Form["Password"];
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
            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;

            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel _registrationModel)
        {
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
            var bannerdata = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerdata = bannerdata;

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
                        ModelState.Clear();
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
        public IActionResult PlatformLandingPage(string? subcats_id, string? filtercity, string? filtercountry, string? filterskill, string? searching, string? filter, string? sortOrder, int? page = 1, int? pageSize = 6, int id = 0)
        {
            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToAction("Index", "Home");

            }


            else
            {

                var missionmedia = _homeRepository.GetMissionMedia();
                ViewBag.missionmedia = missionmedia;
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
                //var missionthemexx = _ciplatformDbContext.MissionThemes.Where(a => a.Title.Contains(filter) || filter == null).ToList();

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



                var skillx = _homeRepository.GetSkills();
                ViewBag.MissionSkills = skillx;

                var totalmissions = outputsf.Count();
                ViewBag.Totalmissions = totalmissions;

                var missionapplication = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();
                ViewBag.missionapplication = missionapplication;


                var userslist = _ciplatformDbContext.Users.ToList();
                ViewBag.userslist = userslist;

                var city = _ciplatformDbContext.Cities.ToList();
                ViewBag.city = city;

                var goalmissionlist = _ciplatformDbContext.GoalMissions.ToList();
                ViewBag.goalmissionlist = goalmissionlist;


                // for approved or declined mission

                var usrlst = _ciplatformDbContext.Users.ToList();
                var missionlst = _ciplatformDbContext.Missions.ToList();
                var mapplst = _ciplatformDbContext.MissionApplications.ToList();


                var appliedmissions = from ma in mapplst
                                      join m in missionlst on ma.MissionId equals m.MissionId
                                      where ma.MissionId == m.MissionId
                                      join u in usrlst on ma.UserId equals u.UserId
                                      where ma.UserId == u.UserId

                                      select new
                                      {
                                          ma,
                                          m,
                                          u
                                      };
                ViewBag.appliedmissions = appliedmissions;


                var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
                ViewBag.avatarimg = avatarimg.Avatar;

                return View();
            }

        }

        public IActionResult _Grid(string? subcats_id, string? filtercity, string? filtercountry, string? filterskill, string? searching, string? filter, string? sortOrder, int? page = 1, int? pageSize = 6, int id = 0)
        {

            ViewBag.subcats_id = subcats_id;
            ViewBag.filtercity = filtercity;
            ViewBag.filtercountry = filtercountry;
            ViewBag.filterskill = filterskill;
            var userslist = _ciplatformDbContext.Users.ToList();
            ViewBag.userslist = userslist;
            //
            var missionmedia = _homeRepository.GetMissionMedia();
            ViewBag.missionmedia = missionmedia;
            //
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
                return RedirectToAction("Index", "Home");
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
                    skillidarr = filterskill.Split(",");
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


            //var missionthemexx = _ciplatformDbContext.MissionThemes.Where(a => a.Title.Contains(filter) || filter == null).ToList();

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



            var skillx = _homeRepository.GetSkills();
            ViewBag.MissionSkills = skillx;


            var missionapplication = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();
            ViewBag.missionapplication = missionapplication;


            var city = _ciplatformDbContext.Cities.ToList();
            ViewBag.city = city;


            var goalmissionlist = _ciplatformDbContext.GoalMissions.ToList();
            ViewBag.goalmissionlist = goalmissionlist;



            // for approved or declined mission

            var usrlst = _ciplatformDbContext.Users.ToList();
            var missionlst = _ciplatformDbContext.Missions.ToList();
            var mapplst = _ciplatformDbContext.MissionApplications.ToList();


            var appliedmissions = from ma in mapplst
                                  join m in missionlst on ma.MissionId equals m.MissionId
                                  where ma.MissionId == m.MissionId
                                  join u in usrlst on ma.UserId equals u.UserId
                                  where ma.UserId == u.UserId

                                  select new
                                  {
                                      ma,
                                      m,
                                      u
                                  };
            ViewBag.appliedmissions = appliedmissions;


            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;

            return View();


        }




        public IActionResult _List(string? subcats_id, string? filtercity, string? filtercountry, string? filterskill, string? searching, string? filter, string? sortOrder, int? page = 1, int? pageSize = 6)
        {
            ViewBag.subcats_id = subcats_id;
            ViewBag.filtercity = filtercity;
            ViewBag.filtercountry = filtercountry;
            ViewBag.filterskill = filterskill;


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
                return RedirectToAction("Index", "Home");
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


            //var missionthemexx = _ciplatformDbContext.MissionThemes.Where(a => a.Title.Contains(filter) || filter == null).ToList();

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



            var skillx = _homeRepository.GetSkills();
            ViewBag.MissionSkills = skillx;

            var currentDate = DateTime.Now;
            ViewBag.currentDate = currentDate;

            var missionapplication = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == ids).ToList();
            ViewBag.missionapplication = missionapplication;


            var city = _ciplatformDbContext.Cities.ToList();
            ViewBag.city = city;

            var goalmissionlist = _ciplatformDbContext.GoalMissions.ToList();
            ViewBag.goalmissionlist = goalmissionlist;


            // for approved or declined mission

            var usrlst = _ciplatformDbContext.Users.ToList();
            var missionlst = _ciplatformDbContext.Missions.ToList();
            var mapplst = _ciplatformDbContext.MissionApplications.ToList();


            var appliedmissions = from ma in mapplst
                                  join m in missionlst on ma.MissionId equals m.MissionId
                                  where ma.MissionId == m.MissionId
                                  join u in usrlst on ma.UserId equals u.UserId
                                  where ma.UserId == u.UserId

                                  select new
                                  {
                                      ma,
                                      m,
                                      u
                                  };
            ViewBag.appliedmissions = appliedmissions;


            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;

            var missionmedia = _homeRepository.GetMissionMedia();
            ViewBag.missionmedia = missionmedia;

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
            else
            {

                var ratinguserdata = _ciplatformDbContext.MissionRatings.Where(y => y.MissionId == missionId && y.UserId == userids).ToList();

                var query = from r in ratinguserdata select r;
                foreach (MissionRating r in query)
                {
                    r.Rating = stars.ToString();
                }

                _ciplatformDbContext.SaveChanges();
            }




            //}
            return RedirectToAction("PlatformLandingPage", "Home");
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








        public IActionResult VolunteeringMission(int id, string commenttext, int MissionId, string searching, int? page = 1, int? pageSize = 9, int missionidforapply = 0)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToAction("Index", "Home");

            }

            var missionmedia = _homeRepository.GetMissionMedia();
            ViewBag.missionmedia = missionmedia;

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
            ViewBag.Result2 = result2.Take(3);




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
            var missionapply = _ciplatformDbContext.MissionApplications.Where(a => a.MissionId == id && a.UserId == ids).ToList();

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


            var avgratingdata = _ciplatformDbContext.MissionRatings.Where(a => a.MissionId == id).ToList();
            ViewBag.avgratingdata = avgratingdata;

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

            // for approved or declined mission

            var usrlst = _ciplatformDbContext.Users.ToList();
            var missionlst = _ciplatformDbContext.Missions.ToList();
            var mapplst = _ciplatformDbContext.MissionApplications.ToList();


            var appliedmissions = from ma in mapplst
                                  join m in missionlst on ma.MissionId equals m.MissionId
                                  where ma.MissionId == m.MissionId
                                  join u in usrlst on ma.UserId equals u.UserId
                                  where ma.UserId == u.UserId

                                  select new
                                  {
                                      ma,
                                      m,
                                      u
                                  };
            ViewBag.appliedmissions = appliedmissions;


            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;


            var documentlist = _ciplatformDbContext.MissionDocuments.Where(a => a.MissionId == b).ToList();

            if (documentlist.Count != null)
            {
                ViewBag.documentlist = documentlist;
                ViewBag.Status = 1;
            }
            else
            {

                ViewBag.Status = 0;
            }
            return View();



        }


        public IActionResult StoryListing()
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToAction("Index", "Home");

            }


            var listofstories = _homeRepository.GetStories();
            var user = _homeRepository.GetUsers();
            var mis = _homeRepository.GetMission();
            var res = _homeRepository.GetTable1WithTable2Records();

            ViewBag.listofstories = res;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;

            var missionmedia = _homeRepository.GetMissionMedia();
            ViewBag.missionmedia = missionmedia;

            var missiontheme = _homeRepository.GetMissionThemess();
            ViewBag.missiontheme = missiontheme;

            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;

            return View(listofstories);


        }



        [HttpPost]
        public IActionResult ShareYourStory2(IFormFileCollection files, string? videourl, int missiondd, string storyTitle, string abcd, int id, string desc, DateTime pubDate, string newArray, int selectedOptionId = 0)
        {

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;
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
                    r.Status = "1";
                }

                _ciplatformDbContext.SaveChanges();
            }



            var missionList = _ciplatformDbContext.Missions.ToList();
            long storyids = 0;
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
                        MissionId = missiondd,
                        //Videourl = videourl
                    };

                    _ciplatformDbContext.Stories.Add(story);

                    _ciplatformDbContext.SaveChanges();
                }

                else
                {
                    var query = from s in isstoryexist select s;



                    foreach (Story s in query)
                    {
                        storyids = s.StoryId;
                        s.Title = storyTitle;
                        s.Description = desc;
                        //s.PublishedAt = pubDate;
                        s.VidUrl = videourl;
                        s.UserId = ids;
                        s.MissionId = missiondd;
                    }

                    _ciplatformDbContext.SaveChanges();


                }
            }

            foreach (var file in files)
            {
                string filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var destinationfilePath = Path.Combine("C:\\Users\\tatva\\Desktop\\Final CIP Project\\MVC\\CIPlatform\\CIPlatform\\wwwroot\\NewFolder1\\", filename);
                var destinationpath2 = Path.Combine("/NewFolder1/", filename);
                var finalpath = destinationfilePath;
                using (var filestream = new FileStream(destinationfilePath, FileMode.Create))
                {
                    file.CopyTo(filestream);

                }
                try
                {
                    StoryMedium storyMedium = new StoryMedium
                    {
                        StoryPath = destinationpath2,
                        StoryType = extention,
                        StoryId = storyids
                    };
                    _ciplatformDbContext.StoryMedia.Add(storyMedium);
                    _ciplatformDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

            var retrivedmedia = _ciplatformDbContext.StoryMedia.Where(a => a.StoryId == storyids).ToList();
            ViewBag.retrivedmedia = retrivedmedia;

            var finallist = from a in appliedmission
                            join m in missionList on a.MissionId equals m.MissionId
                            where a.MissionId == m.MissionId

                            select new
                            {
                                a,
                                m
                            };



            ViewBag.finallist = finallist;



            var abc = HttpContext.Session.GetString("userid");
            if (abc == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return RedirectToAction("ShareYourStory", "Home");
        }



        public IActionResult EditStory(int selectedOptionId, string appliedMission)
        {


            var abc = HttpContext.Session.GetString("userid");
            if (abc == null)
            {
                return RedirectToAction("Login", "Login");
            }
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


            var retrivedmedia = new List<StoryMedium>();
            //var retrieve = _ciplatformDbContext.MissionMedia.Where(i => i.MissionId == selectedOptionId).ToList().FirstOrDefault();
            var imgpaths = new List<string>();
            if (data != null)
            {
                retrivedmedia = _ciplatformDbContext.StoryMedia.Where(a => a.StoryId == data.StoryId).ToList();

                var Storydetails = new Story

                {

                    Title = data.Title,
                    Description = data.Description,
                    PublishedAt = data.PublishedAt,
                    VidUrl = data.VidUrl,
                    UserId = ids,
                    MissionId = selectedOptionId,
                    //storypath = retrivedmedia.StoryPath


                };

                foreach (var b in retrivedmedia)
                {
                    imgpaths.Add(b.StoryPath);

                }


                return Json(new { obj1 = Storydetails, obj2 = imgpaths });





            }
            else
            {
                ViewBag.exist = 0;
                ViewBag.checkempty = 0;
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


        public IActionResult StoryDetails(int id)
        {
            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToAction("Index", "Home");

            }


            var specificStory = _homeRepository.GetSpecificStory(id);
            var mis = _homeRepository.GetMission();
            var user = _homeRepository.GetUsers();
            ViewBag.specificStory = specificStory;

            var storymedia = _ciplatformDbContext.StoryMedia.Where(a => a.StoryId == id).ToList();
            ViewBag.storymedia = storymedia;
            //recom to co-w

            ViewBag.user = user;

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;

            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;

            var storylst = _ciplatformDbContext.Stories.ToList();
            ViewBag.storylst = storylst;



            var listofstories = _homeRepository.GetStories();

            var res = _homeRepository.GetTable1WithTable2Records();

            ViewBag.listofstories = res;

            var missionmedia = _homeRepository.GetMissionMedia();
            ViewBag.missionmedia = missionmedia;

            return View();
        }

        public IActionResult ShareYourStory(int missiondd, string videourl, string storyTitle, string abcd, int id, string desc, DateTime pubDate, string newArray, int selectedOptionId = 0)
        {

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);

            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;
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
                    r.Status = "1";
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
                        // PublishedAt = pubDate,
                        UserId = ids,
                        MissionId = missiondd,
                        VidUrl = videourl
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
                        //s.VidUrl = videourl;
                        s.UserId = ids;
                        s.MissionId = missiondd;
                    }

                    _ciplatformDbContext.SaveChanges();


                }
            }



            var finallist = from a in appliedmission
                            join m in missionList on a.MissionId equals m.MissionId
                            where a.MissionId == m.MissionId

                            select new
                            {
                                a,
                                m
                            };



            ViewBag.finallist = finallist;

            if (newArray != null)
            {

                var a = Request.Form.Files.ToArray();
                string[] imgdata = newArray.Split(",", StringSplitOptions.RemoveEmptyEntries);


                // Save the images to disk or database
                foreach (var image in imgdata)
                {

                    var fileName = Path.GetFileName(image);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "~/NewFolder1/", fileName);
                    System.IO.File.Copy(image, filePath, true);

                    string substring = filePath.Substring(63);
                    string imgname = filePath.Substring(74);

                    var v = Path.GetDirectoryName(image);
                    Console.WriteLine(v);





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



        public IActionResult EditProfile(int countryid, int skillsid, string subject, string msg)
        {
            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToAction("Index", "Home");

            }

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;


            var countrylist = _ciplatformDbContext.Countries.ToList();
            ViewBag.countrylist = countrylist;
            if (countryid != 0)
            {
                var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
                ViewBag.citylist = citylist;
            }
            else
            {

                var citylist = _ciplatformDbContext.Cities.ToList();
                ViewBag.citylist = citylist;
            }



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

            }

            else
            {

                var updateddata = _ciplatformDbContext.Users.Where(a => a.UserId == ids).ToList();

                var query = from u in updateddata select u;

                foreach (var a in updateddata)
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

                _ciplatformDbContext.SaveChanges();
            }



            var skillsList = _homeRepository.GetSkills();
            ViewBag.skills = skillsList;




            var skillList = _ciplatformDbContext.Skills.ToList();
            var userskillsList = _ciplatformDbContext.UserSkills.ToList();

            var userSkills = from s in skillsList
                             join us in userskillsList on s.SkillId equals us.SkillId
                             where s.SkillId == us.SkillId
                             select new
                             {
                                 s.SkillName

                             };

            ViewBag.userSkills = userSkills;


            // for my profile check

            ViewBag.data = data;

            // CONTACT US



            var emailadd = _homeRepository.GetUserEmail(ids);
            ViewBag.emailadd = emailadd;





            var contData = _ciplatformDbContext.ContactUs.Where(y => y.UserId == ids).ToList();

            if (msg != null)
            {
                if (contData.Count == 0)
                {
                    var cont = new ContactU
                    {

                        Subject = subject,
                        Message = msg,
                        UserId = ids,

                    };

                    _ciplatformDbContext.ContactUs.Add(cont);

                    _ciplatformDbContext.SaveChanges();
                }
            }

            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;

            return View();







        }

        public List<string> Usereditprofileupdate(int countryid, string cityname)
        {

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);





            var countrylist = _ciplatformDbContext.Countries.ToList();
            ViewBag.countrylist = countrylist;

            var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
            ViewBag.citylist = citylist;


            List<string> citys = new List<string>();
            foreach (var a in citylist)
            {

                citys.Add(a.Name);
            }



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
                    ViewBag.Cityname = cityname;

                }





            }

            return citys;

        }


        [HttpPost]
        public IActionResult SaveUserData(string availability, int countryid, string cityname, string[] skillids, string firstname, string lastname, int id, string empId, string title, string dept, string profile, string whyI, int cityId, string linkedInurl)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);

            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;


            long cityid = 0;
            if (cityname != null)
            {
                var speccity = _ciplatformDbContext.Cities.FirstOrDefault(a => a.Name.Contains(cityname));
                cityid = speccity.CityId;
            }

            if (firstname != null && lastname != null && cityid != 0 && countryid != 0)
            {
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
                    u.CityId = cityid;
                    u.LinkedInUrl = linkedInurl;
                    u.UserId = ids;
                    u.CountryId = countryid;
                    u.UserAvailability = availability;


                }
                _ciplatformDbContext.SaveChanges();

            }

            var allskillss = _ciplatformDbContext.UserSkills.ToList();
            _ciplatformDbContext.UserSkills.RemoveRange(allskillss);

            foreach (var s in skillids)
            {

                var skillid = Convert.ToInt32(s);

                var userSkills = _ciplatformDbContext.UserSkills.Where(a => a.SkillId == skillid && a.UserId == ids).ToList();

                if (userSkills.Count == 0)
                {
                    var allskills = new UserSkill
                    {

                        SkillId = Convert.ToInt32(s),
                        UserId = ids

                    };

                    _ciplatformDbContext.UserSkills.Add(allskills);

                    _ciplatformDbContext.SaveChanges();
                }

            }

            TempData["citydata"] = cityid;
            TempData["countrydata"] = countryid;


            var uid = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var data1 = _ciplatformDbContext.Users.FirstOrDefault(a => a.UserId == uid);
            //if (data1.CountryId != 0 || data1.CityId != 0)
            //{

            //    return RedirectToAction("PlatformLandingPage", "Home");
            //}
            //else {
            //    return RedirectToAction("EditProfile", "Home");

            //}
            return RedirectToAction("EditProfile", "Home");
        }


        public IActionResult ChangeUserPass(string newpass)
        {

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);



            var user = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));

            if (user != null)
            {

                user.Password = newpass;
                _ciplatformDbContext.Users.Update(user);
                _ciplatformDbContext.SaveChanges();


                return RedirectToAction("EditProfile", "Home");




            }
            else
            {

                return RedirectToAction("EditProfile", "Home");

            }


        }





        public IActionResult PrivacyPolicy()
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;


            var avatarimg = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids));
            ViewBag.avatarimg = avatarimg.Avatar;

            return View();
        }

        public IActionResult VolTimesheet(string finaltime, int id, int missionId, DateTime date, int selectedOptionId, int hours, int mins, string msg, int action, DateTime datevol, string goalMsg)
        {

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);

            var missionappList = _homeRepository.GetMissionAppList();
            var missionList = _homeRepository.GetMission();
            var appliedMissions = _homeRepository.Getappliedmissions(ids);
            ViewBag.appliedMissions = appliedMissions;


            var missionlst = _ciplatformDbContext.Missions.ToList();

            var missionapplst = _ciplatformDbContext.MissionApplications.ToList();
            ////var goalList = _homeRepository.GetGoalMissions();
            ////var goalmissions = _homeRepository.GetAllGoalMissions();
            var goalList = _ciplatformDbContext.GoalMissions.ToList();


            var goalmissions = from g in goalList
                               join m in missionlst on g.MissionId equals m.MissionId
                               where g.MissionId == m.MissionId
                               join am in missionapplst on g.MissionId equals am.MissionId
                               where g.MissionId == am.MissionId
                               select new
                               {
                                   g.MissionId,
                                   m,
                                   m.Title
                               };
            ViewBag.goalmissions = goalmissions;



            var timeData = _ciplatformDbContext.Timesheets.Where(y => y.UserId == ids && y.MissionId == selectedOptionId).ToList();

            // for time based 

            string time = hours.ToString() + ":" + mins.ToString();

            if (selectedOptionId != 0 && date != null && hours != 0 && mins != 0 && msg != null)
            {
                if (timeData.Count == 0)
                {
                    var data = new Timesheet
                    {

                        MissionId = selectedOptionId,
                        TimesheetTime = TimeOnly.Parse(time),
                        DateVolunteered = date,
                        Notes = msg,
                        UserId = ids

                    };

                    _ciplatformDbContext.Timesheets.Add(data);

                    _ciplatformDbContext.SaveChanges();
                }
            }



            var timelist = _ciplatformDbContext.Timesheets.ToList();

            var timesheetAllData = from t in timelist
                                   join m in missionlst on t.MissionId equals m.MissionId
                                   where t.MissionId == m.MissionId

                                   select new
                                   {
                                       t,
                                       t.TimesheetTime,
                                       m,
                                       m.Title,
                                       m.StartDate,
                                       m.EndDate
                                   };
            ViewBag.timesheetAllData = timesheetAllData;


            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].TimesheetTime != null)
                {



                    finaltime = timelist[i].TimesheetTime.ToString();

                    TimeOnly splitedtime = TimeOnly.Parse(finaltime); // parse the time string into a TimeSpan object

                    int hrs = splitedtime.Hour; // get the hours component
                    int minutes = splitedtime.Minute;

                    ViewBag.hours = hrs;
                    ViewBag.minutes = minutes;

                }
            }



            // for goal based

            if (selectedOptionId != 0 && action != null && datevol != null && goalMsg != null)
            {
                if (timeData.Count == 0)
                {
                    var goaldata = new Timesheet
                    {

                        MissionId = selectedOptionId,
                        Action = action,
                        DateVolunteered = datevol,
                        Notes = goalMsg,
                        UserId = ids

                    };

                    _ciplatformDbContext.Timesheets.Add(goaldata);

                    _ciplatformDbContext.SaveChanges();
                }
            }

            // remove data Vol time

            var timesheetSavedData = _ciplatformDbContext.Timesheets.FirstOrDefault(id => id.MissionId == missionId);

            if (timesheetSavedData != null)
            {


                _ciplatformDbContext.Timesheets.Remove(timesheetSavedData);
                _ciplatformDbContext.SaveChanges();
            }

            return View();
        }

        public IActionResult EditVolTime(int selectedOptionId)
        {
            //edit vol time

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);


            var missionappList = _homeRepository.GetMissionAppList();
            var missionList = _homeRepository.GetMission();
            var appliedMissions = _homeRepository.Getappliedmissions(ids);
            ViewBag.appliedMissions = appliedMissions;


            var missionlst = _ciplatformDbContext.Missions.ToList();

            var missionapplst = _ciplatformDbContext.MissionApplications.ToList();

            var goalList = _ciplatformDbContext.GoalMissions.ToList();


            var goalmissions = from g in goalList
                               join m in missionlst on g.MissionId equals m.MissionId
                               where g.MissionId == m.MissionId
                               join am in missionapplst on g.MissionId equals am.MissionId
                               where g.MissionId == am.MissionId
                               select new
                               {
                                   g.MissionId,
                                   m,
                                   m.Title
                               };
            ViewBag.goalmissions = goalmissions;




            var data = _ciplatformDbContext.Timesheets.Where(a => a.UserId == ids && a.MissionId == selectedOptionId).ToList().FirstOrDefault();



            if (data != null)
            {

                var fetchVolTimeDetails = new Timesheet
                {

                    DateVolunteered = data.DateVolunteered,
                    TimesheetTime = data.TimesheetTime,
                    Notes = data.Notes,
                    UserId = ids,
                    MissionId = selectedOptionId
                };




                return Json(fetchVolTimeDetails);
            }
            else
            {


            }

            return View();




        }




        public IActionResult SaveVolTimeData(DateTime date1, int selectedOptionId, int hours1, int mins1, string msg1, int action, DateTime datevol, string goalMsg)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);


            // for time based 

            string time = hours1.ToString() + ":" + mins1.ToString();

            var timelist = _ciplatformDbContext.Timesheets.ToList();


            string finaltime = timelist[0].TimesheetTime.ToString();




            TimeOnly splitedtime = TimeOnly.Parse(finaltime);

            int hrs = splitedtime.Hour;
            int minutes = splitedtime.Minute;

            ViewBag.hours = hrs;
            ViewBag.minutes = minutes;



            var timeData = _ciplatformDbContext.Timesheets.Where(y => y.UserId == ids && y.MissionId == selectedOptionId).ToList();

            var query = from r in timeData select r;
            foreach (Timesheet r in query)
            {
                r.TimesheetTime = TimeOnly.Parse(time);
                r.DateVolunteered = date1;
                r.Notes = msg1;
                r.MissionId = selectedOptionId;
                r.UserId = ids;

            }

            _ciplatformDbContext.SaveChanges();




            return RedirectToAction("VolTimesheet", "Home");
        }



        // GOAL VOL T

        public IActionResult EditVolGoal(int selectedOptionId)
        {
            //edit vol time

            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);


            var missionappList = _homeRepository.GetMissionAppList();
            var missionList = _homeRepository.GetMission();
            var appliedMissions = _homeRepository.Getappliedmissions(ids);
            ViewBag.appliedMissions = appliedMissions;


            var missionlst = _ciplatformDbContext.Missions.ToList();

            var missionapplst = _ciplatformDbContext.MissionApplications.ToList();

            var goalList = _ciplatformDbContext.GoalMissions.ToList();


            var goalmissions = from g in goalList
                               join m in missionlst on g.MissionId equals m.MissionId
                               where g.MissionId == m.MissionId
                               join am in missionapplst on g.MissionId equals am.MissionId
                               where g.MissionId == am.MissionId
                               select new
                               {
                                   g.MissionId,
                                   m,
                                   m.Title
                               };
            ViewBag.goalmissions = goalmissions;




            var data = _ciplatformDbContext.Timesheets.Where(a => a.UserId == ids && a.MissionId == selectedOptionId).ToList().FirstOrDefault();



            if (data != null)
            {

                var fetchVolGoalDetails = new Timesheet
                {

                    DateVolunteered = data.DateVolunteered,
                    Action = data.Action,
                    Notes = data.Notes,
                    UserId = ids,
                    MissionId = selectedOptionId
                };



                return Json(fetchVolGoalDetails);
            }


            return View();




        }




        public IActionResult SaveVolGoalData(DateTime goaldate, int selectedOptionId, int action1, string goalmsg1)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);



            var GoalData = _ciplatformDbContext.Timesheets.Where(y => y.UserId == ids && y.MissionId == selectedOptionId).ToList();

            var query = from r in GoalData select r;
            foreach (Timesheet r in query)
            {

                r.DateVolunteered = goaldate;
                r.Notes = goalmsg1;
                r.Action = action1;
                r.MissionId = selectedOptionId;
                r.UserId = ids;

            }

            _ciplatformDbContext.SaveChanges();




            return RedirectToAction("VolTimesheet", "Home");
        }





        //public IActionResult Admin_user(int userid, string fname, string lname, string email, string pass, string avtar, string empid, string dept, string cityid, int countryid, string protxt, string status)
        // {

        //    // Add user

        //    var userData = _ciplatformDbContext.Users.Where(a => a.Email == email).ToList();

        //    if (fname != null && lname != null && email != null && pass != null && cityid != null && countryid != 0 && status != null)
        //    {
        //        var citydata = _ciplatformDbContext.Cities.FirstOrDefault(a => a.Name == cityid);
        //        var cid = citydata.CityId;
        //        if (userData.Count == 0)
        //        {
        //            var userdetails = new User
        //            {

        //                FirstName = fname,
        //                LastName = lname,
        //                Email = email,
        //                Password = pass,
        //                Avatar = avtar,
        //                EmployeeId = empid,
        //                Department = dept,
        //                CityId = cid,
        //                CountryId = countryid,
        //                ProfileText = protxt,
        //                Status = status


        //            };

        //            _ciplatformDbContext.Users.Add(userdetails);

        //            _ciplatformDbContext.SaveChanges();
        //        }
        //    }




        //    var users = _homeRepository.GetUsers();
        //    ViewBag.users = users;

        //    // delete user

        //    if (userid != 0)
        //    {
        //        _homeRepository.deleteusers(userid);


        //    }


        //    // for city country

        //    var countrylist = _ciplatformDbContext.Countries.ToList();
        //    ViewBag.countrylist = countrylist;
        //    if (countryid != 0)
        //    {
        //        var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
        //        ViewBag.citylist = citylist;
        //    }
        //    else
        //    {

        //        var citylist = _ciplatformDbContext.Cities.ToList();
        //        ViewBag.citylist = citylist;
        //    }



        //    return View();
        //}



        public IActionResult Admin_user(int userid, string fname, string lname, string email, string pass, string avtar, string empid, string dept, int cityid, int countryid, string protxt, string status)
        {

            var auth = HttpContext.Session.GetString("credentials");
            if (auth != null)
            {


                var users = _homeRepository.GetUsers();
                ViewBag.users = users;

                // delete user

                if (userid != 0)
                {
                    _homeRepository.deleteusers(userid);


                }

                return View();
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult AddAdminuser(AdminUserVM adminuservmodel)
        {



            var adminuservmodel2 = new User
            {

                FirstName = adminuservmodel.FirstName,
                LastName = adminuservmodel.LastName,
                Email = adminuservmodel.Email,
                Password = adminuservmodel.Password,
                Avatar = adminuservmodel.Avatar,
                EmployeeId = adminuservmodel.EmployeeId,
                Department = adminuservmodel.Department,
                //CityId = adminuservmodel.CityId,
                //CountryId = adminuservmodel.CountryId,
                ProfileText = adminuservmodel.ProfileText,
                Status = adminuservmodel.Status


            };

            _ciplatformDbContext.Users.Add(adminuservmodel2);

            _ciplatformDbContext.SaveChanges();


            TempData["DataSaved"] = "User added successfully";

            return RedirectToAction("Admin_user");


        }
        public List<string> Admin_userCityCountry(int countryid, string cityname)
        {

            var countrylist = _ciplatformDbContext.Countries.ToList();
            ViewBag.countrylist = countrylist;

            var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
            ViewBag.citylist = citylist;


            List<string> citys = new List<string>();
            foreach (var a in citylist)
            {

                citys.Add(a.Name);
            }



            return citys;

        }


        public IActionResult EditAdminUser(int uId)
        {
            var data = _ciplatformDbContext.Users.Where(a => a.UserId == uId).ToList().FirstOrDefault();

            if (data != null)
            {

                var fetchUserdetails = new User
                {

                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    Password = data.Password,
                    Avatar = data.Avatar,
                    CityId = data.CityId,
                    CountryId = data.CountryId,
                    ProfileText = data.ProfileText,
                    Status = data.Status,
                    EmployeeId = data.EmployeeId,
                    Department = data.Department,
                    UserId = uId

                };




                return Json(fetchUserdetails);


                //return View();
            }
            else
            {
                return View();
            }
        }


        public IActionResult SaveAdminUserData(int uid, string fname, string lname, string email, string pass, string avtar, string empid, string dept, int cityid, int countryid, string protxt, string status)
        {

            var adminUserData = _ciplatformDbContext.Users.Where(y => y.UserId == uid).ToList();

            var query = from r in adminUserData select r;

            foreach (User r in query)
            {

                r.FirstName = fname;
                r.LastName = lname;
                r.Email = email;
                r.Password = pass;
                r.Avatar = avtar;
                r.CityId = cityid;
                r.CountryId = countryid;
                r.ProfileText = protxt;
                r.Status = status;
                r.EmployeeId = empid;
                r.Department = dept;
                r.UserId = uid;

            }

            _ciplatformDbContext.SaveChanges();
            return View();
        }


        public IActionResult CMSPage(int cmsid)
        {
            var auth = HttpContext.Session.GetString("credentials");
            if (auth != null)
            {

                var cmsData = _homeRepository.GetCMSData();
                ViewBag.cmsData = cmsData;


                // delete data


                var cmsSavedData = _ciplatformDbContext.CmsPages.FirstOrDefault(id => id.CmsPageId == cmsid);
                ModelState.Clear();
                if (cmsSavedData != null)
                {


                    _ciplatformDbContext.CmsPages.Remove(cmsSavedData);
                    _ciplatformDbContext.SaveChanges();
                }



                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public IActionResult AddCMS(CMSPageVM _cms)
        {
            var cmsdetails = _ciplatformDbContext.CmsPages.Where(a => a.Title == _cms.Title);

            if (_cms.Title != null && _cms.Description != null && _cms.Slug != null && _cms.Status != null)
            {


                if (cmsdetails.Count() == 0)
                {
                    var cmsdata = new CmsPage()
                    {
                        Title = _cms.Title,
                        Description = _cms.Description,
                        Slug = _cms.Slug,
                        Status = _cms.Status


                    };
                    _ciplatformDbContext.CmsPages.Add(cmsdata);
                    _ciplatformDbContext.SaveChanges();



                }

            }

            TempData["DataSaved"] = "Data added successfully";

            return RedirectToAction("CMSPage", "Home");
        }

        public IActionResult EditCMSData(int CMSId)
        {
            var data = _ciplatformDbContext.CmsPages.Where(a => a.CmsPageId == CMSId).ToList().FirstOrDefault();

            if (data != null)
            {

                var fetchCMSdetails = new CmsPage
                {

                    Title = data.Title,
                    Description = data.Description,
                    Slug = data.Slug,
                    Status = data.Status,
                    CmsPageId = CMSId,


                };

                return Json(fetchCMSdetails);



            }
            else
            {
                return View();
            }
        }



        public IActionResult SaveCMSData(int cmsId, string title, string desc, string slug, string status)
        {

            var cmsData = _ciplatformDbContext.CmsPages.Where(y => y.CmsPageId == cmsId).ToList();

            var query = from r in cmsData select r;

            if (title != null && desc != null && slug != null && status != null)
            {
                foreach (CmsPage r in query)
                {

                    r.Title = title;
                    r.Description = desc;
                    r.Slug = slug;
                    r.Status = status;
                    r.CmsPageId = cmsId;

                }
            }
            _ciplatformDbContext.SaveChanges();

            return View();
        }


        public IActionResult Admin_MissionTheme(int mtId)
        {
            var auth = HttpContext.Session.GetString("credentials");
            if (auth != null)
            {
                var missionTh = _homeRepository.GetMissionThemes();
                ViewBag.missionTh = missionTh;


                // delete data

                var missiontheme = _ciplatformDbContext.MissionThemes.Include(u => u.Missions).Where(u => u.MissionThemeId == mtId).ToList();

                var mt = _ciplatformDbContext.MissionThemes.FirstOrDefault(missiontheme => missiontheme.MissionThemeId == mtId);
                ModelState.Clear();
                if (mt != null)
                {
                    mt.DeletedAt = DateTime.Now;
                    _ciplatformDbContext.SaveChanges();
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Admin_AddMTheme(Admin_MTVM _adMT)
        {
            var mtdetails = _ciplatformDbContext.MissionThemes.Where(a => a.Title == _adMT.Title);

            if (_adMT.Title != null && _adMT.Status != 0)
            {


                if (mtdetails.Count() == 0)
                {
                    var mtdata = new MissionTheme()
                    {
                        Title = _adMT.Title,
                        Status = (byte)_adMT.Status


                    };
                    _ciplatformDbContext.MissionThemes.Add(mtdata);
                    _ciplatformDbContext.SaveChanges();



                }

            }
            TempData["DataSaved"] = "Theme added successfully";
            TempData["Error"] = "Error while saving your data";

            return RedirectToAction("Admin_MissionTheme", "Home");
        }

        public IActionResult EditMTData(int mtid)
        {
            var data = _ciplatformDbContext.MissionThemes.Where(a => a.MissionThemeId == mtid).ToList().FirstOrDefault();

            if (data != null)
            {

                var fetchMTdetails = new MissionTheme
                {

                    Title = data.Title,
                    Status = data.Status,
                    MissionThemeId = mtid


                };

                ViewBag.Status = 1;

                TempData["EditedData"] = "Theme updated successfully";


                return Json(fetchMTdetails);



            }
            else
            {
                return View();
            }
        }


        public IActionResult SaveMTData(int mtid, string title, int status)
        {

            var mthemeData = _ciplatformDbContext.MissionThemes.Where(y => y.MissionThemeId == mtid).ToList();

            var query = from r in mthemeData select r;

            if (title != null && status != null)
            {
                foreach (MissionTheme r in query)
                {

                    r.Title = title;
                    r.Status = (byte)status;
                    r.MissionThemeId = mtid;

                }
            }
            _ciplatformDbContext.SaveChanges();
            TempData["DataSavedMessage"] = "Data has been saved successfully!";
            return RedirectToAction("Admin_MissionTheme", "Home");
        }





        public IActionResult Admin_MissionApp(Admin_MAppVM _mapp)
        {
            var auth = HttpContext.Session.GetString("credentials");
            if (auth != null)
            {
                var usrlst = _ciplatformDbContext.Users.ToList();
                var missionlst = _ciplatformDbContext.Missions.ToList();
                var mapplst = _ciplatformDbContext.MissionApplications.ToList();


                var appliedmissions = from ma in mapplst
                                      join m in missionlst on ma.MissionId equals m.MissionId
                                      where ma.MissionId == m.MissionId
                                      join u in usrlst on ma.UserId equals u.UserId
                                      where ma.UserId == u.UserId

                                      select new
                                      {
                                          ma,
                                          m,
                                          u
                                      };
                ViewBag.appliedmissions = appliedmissions;

                TempData["Approved"] = "Approved";
                TempData["Declined"] = "Declined";


                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Approved(int missionId, int uid)
        {
            var mappdetails = _ciplatformDbContext.MissionApplications.Where(a => a.MissionId == missionId && a.UserId == uid);



            var query = from r in mappdetails select r;

            foreach (MissionApplication r in query)
            {

                r.ApprovalStatus = "approved";
                r.UserId = uid;
                r.MissionId = missionId;

            }

            _ciplatformDbContext.SaveChanges();

            TempData["Approved"] = "Approved";

            return RedirectToAction("Admin_MissionApp", "Home");
        }



        public IActionResult Declined(int missionId, int uid)
        {
            var mappdetails = _ciplatformDbContext.MissionApplications.Where(a => a.MissionId == missionId && a.UserId == uid);

            var query = from r in mappdetails select r;

            foreach (MissionApplication r in query)
            {

                r.ApprovalStatus = "declined";
                r.UserId = uid;
                r.MissionId = missionId;

            }

            _ciplatformDbContext.SaveChanges();

            TempData["Declined"] = "Declined";


            return RedirectToAction("Admin_MissionApp", "Home");
        }


        public IActionResult Admin_MissionSkills(int selectedOptionId)
        {
            var missionlst = _homeRepository.GetMission();
            ViewBag.missionlst = missionlst;

            var missionskills = _homeRepository.GetSkillandMissionSkill();
            ViewBag.missionskills = missionskills;


            // autofill mission skills

            var skillsList = _homeRepository.GetSkills();
            ViewBag.skills = skillsList;



            return View();
        }


        public IActionResult SaveMSData(int selectedOptionId, string[] skillids, string status)
        {
            // Add mission skills
            var allskillss = _ciplatformDbContext.MissionSkills.ToList();
            _ciplatformDbContext.MissionSkills.RemoveRange(allskillss);

            if (skillids != null && status != null)
            {
                foreach (var s in skillids)
                {

                    var skillid = Convert.ToInt32(s);

                    var mskillsData = _ciplatformDbContext.MissionSkills.Where(m => m.SkillId == skillid && m.MissionId == selectedOptionId).ToList();

                    if (mskillsData.Count == 0)
                    {
                        var allskills = new MissionSkill
                        {

                            SkillId = Convert.ToInt32(s),
                            MissionId = selectedOptionId

                        };

                        _ciplatformDbContext.MissionSkills.Add(allskills);

                        _ciplatformDbContext.SaveChanges();
                    }



                }
            }


            return RedirectToAction("Admin_MissionSkills", "Home");
        }



        public IActionResult Admin_Skills(int skillsId)
        {
            var auth = HttpContext.Session.GetString("credentials");
            if (auth != null)
            {

                // display data

                var skills = _ciplatformDbContext.Skills.ToList();
                ViewBag.skills = skills;



                // delete data

                //var skillsSavedData = _ciplatformDbContext.Skills.Where(id => id.SkillId == skillsId).FirstOrDefault();
                //ModelState.Clear();
                //if (skillsSavedData != null)
                //{

                //    _ciplatformDbContext.Skills.Remove(skillsSavedData);
                //    _ciplatformDbContext.SaveChanges();
                //}


                var skillsdata = _ciplatformDbContext.Skills.Include(u => u.MissionSkills).Include(u => u.UserSkills).Where(u => u.SkillId == skillsId).ToList();

                var skillsSavedData = _ciplatformDbContext.Skills.Where(skillsdata => skillsdata.SkillId == skillsId).FirstOrDefault();

                ModelState.Clear();

                if (skillsSavedData != null)
                {
                    skillsSavedData.DeletedAt = DateTime.Now;
                    _ciplatformDbContext.SaveChanges();
                }



                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Admin_AddSkills(Admin_SkillsVM _skills)
        {
            //Add Skills

            var skillsdetails = _ciplatformDbContext.Skills.Where(a => a.SkillName == _skills.SkillName);

            if (_skills.SkillName != null && _skills.Status != null)
            {


                if (skillsdetails.Count() == 0)
                {
                    var skillsdata = new Skill()
                    {
                        SkillName = _skills.SkillName,
                        Status = _skills.Status


                    };
                    _ciplatformDbContext.Skills.Add(skillsdata);
                    _ciplatformDbContext.SaveChanges();

                }

            }


            TempData["DataSaved"] = "Skills added successfully";

            return RedirectToAction("Admin_Skills", "Home");

        }
        public IActionResult EditSkillsData(int skillId)
        {
            var data = _ciplatformDbContext.Skills.Where(a => a.SkillId == skillId).ToList().FirstOrDefault();

            if (data != null)
            {

                var fetchSkillsdetails = new Skill
                {

                    SkillName = data.SkillName,
                    Status = data.Status,
                    SkillId = skillId


                };

                return Json(fetchSkillsdetails);



            }
            else
            {
                return View();
            }
        }


        public IActionResult SaveSkillsData(int skillsId, string skillsname, string status)
        {

            var skillsData = _ciplatformDbContext.Skills.Where(y => y.SkillId == skillsId).ToList();

            var query = from r in skillsData select r;

            if (skillsname != null && status != null)
            {
                foreach (Skill r in query)
                {

                    r.SkillName = skillsname;
                    r.Status = status;
                    r.SkillId = skillsId;

                }
            }
            _ciplatformDbContext.SaveChanges();

            TempData["DataSavedMessage"] = "Data has been saved successfully!";

            return RedirectToAction("Admin_Skills", "Home");

        }


        public IActionResult Admin_Story(Admin_Story _story, int storyid)
        {

            var auth = HttpContext.Session.GetString("credentials");
            if (auth != null)
            {

                var storydata = _homeRepository.GetTable1WithTable2Records();
                ViewBag.storydata = storydata;

                var missionlst = _homeRepository.GetMission();
                ViewBag.missionlst = missionlst;


                // delete story data


                var storySavedData = _ciplatformDbContext.Stories.FirstOrDefault(id => id.StoryId == storyid);
                var storymedia = _ciplatformDbContext.StoryMedia.Where(stories => stories.StoryId == storyid).ToList();
                ModelState.Clear();
                if (storySavedData != null)
                {

                    if (storymedia.Count != 0)
                    {
                        foreach (var story in storymedia)
                        {
                            _ciplatformDbContext.StoryMedia.Remove(story);
                        }
                    }
                    _ciplatformDbContext.SaveChanges();
                    _ciplatformDbContext.Stories.Remove(storySavedData);

                    _ciplatformDbContext.SaveChanges();
                }


                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ViewStoryDetails(int storyId, Admin_Story _story)
        {
            // for story

            var usrlst = _ciplatformDbContext.Users.ToList();
            var missionlst = _ciplatformDbContext.Missions.ToList();
            var storylst = _ciplatformDbContext.Stories.ToList();
            var storymedialst = _ciplatformDbContext.StoryMedia.Where(a => a.StoryId == storyId).ToList();

            ViewBag.storymedialst = storymedialst;
            var imgpaths = new List<string>();
            foreach (var a in storymedialst)
            {
                imgpaths.Add(a.StoryPath);

            }
            var stories = from s in storylst
                          join m in missionlst on s.MissionId equals m.MissionId
                          where s.MissionId == m.MissionId
                          join u in usrlst on s.UserId equals u.UserId
                          where s.UserId == u.UserId
                          join sm in storymedialst on s.StoryId equals sm.StoryId
                          where s.StoryId == sm.StoryId

                          select new
                          {
                              s,
                              m,
                              u,
                              sm
                          };

            var storytitles = "";
            var missiontitle = "";
            var storydesc = "";
            var storymedia = "";

            var storylist = stories.ToList();
            foreach (var itm in storylist)
            {
                storytitles = itm.s.Title;
                missiontitle = itm.m.Title;
                storydesc = itm.s.Description;
                storymedia = itm.sm.StoryPath;

            }

            Admin_Story storymodel = new Admin_Story();
            storymodel.StoryTitle = storytitles;
            storymodel.MissionTitle = missiontitle;
            storymodel.Description = storydesc;
            storymodel.StoryPath = storymedia;



            //return Json(storymodel);
            return Json(new { obj1 = storymodel, obj2 = imgpaths });
        }

        public IActionResult ApprovedStory(int missionId, int uid)
        {
            var storydetails = _ciplatformDbContext.Stories.Where(a => a.MissionId == missionId && a.UserId == uid);



            var query = from r in storydetails select r;

            foreach (Story r in query)
            {

                r.Status = "approved";
                r.UserId = uid;
                r.MissionId = missionId;

            }

            _ciplatformDbContext.SaveChanges();



            return RedirectToAction("Admin_Story", "Home");
        }

        public IActionResult DeclinedStory(int missionId, int uid)
        {
            var storydetails = _ciplatformDbContext.Stories.Where(a => a.MissionId == missionId && a.UserId == uid);

            var query = from r in storydetails select r;

            foreach (Story r in query)
            {

                r.Status = "declined";
                r.UserId = uid;
                r.MissionId = missionId;

            }

            _ciplatformDbContext.SaveChanges();



            return RedirectToAction("Admin_Story", "Home");
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


        public IActionResult Timesheet()
        {
            var userId = HttpContext.Session.GetString("userid");
            var missions = _ciplatformDbContext.Missions.ToList();
            var user = _ciplatformDbContext.Users.Include(u => u.Timesheets).Include(u => u.MissionApplications).FirstOrDefault(u => u.UserId == Convert.ToInt64(userId));
            var TsData = _ciplatformDbContext.Timesheets.Where(a => a.UserId == Convert.ToInt64(userId));
            var MissionApplications = _ciplatformDbContext.MissionApplications.Where(a => a.UserId == Convert.ToInt64(userId));

            List<Mission> GoalMissions = new List<Mission>();
            List<Mission> TimeMissions = new List<Mission>();
            List<Mission> appliedMissions = new List<Mission>();
            List<Timesheet> TsByTime = new List<Timesheet>();
            List<Timesheet> TsByGoal = new List<Timesheet>();

            foreach (var mission in MissionApplications)
            {
                appliedMissions.AddRange(missions.Where(m => m.MissionId == mission.MissionId));
            }


            foreach (var mission in appliedMissions)
            {
                if (mission.MissionType == "Goal")
                {
                    GoalMissions.Add(mission);
                }
                else
                {
                    TimeMissions.Add(mission);
                }
            }

            foreach (var Ts in TsData)
            {
                if (GoalMissions.Any(gm => gm.MissionId == Ts.MissionId))
                {
                    TsByGoal.Add(Ts);
                }
                else
                {
                    TsByTime.Add(Ts);
                }
            }




            var timesheetVM = new TimesheetVM
            {
                TimeTs = TsByTime,
                GoalTs = TsByGoal,
                GoalMissionsList = GoalMissions,
                TimeMissionsList = TimeMissions,

            };


            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var fullname = _homeRepository.GetLoginUser(ids);
            ViewBag.fullname = fullname;



            return View(timesheetVM);
        }



        #region TimesheetPost
        [HttpPost]
        public bool TimesheetForTime(long timesheetId, string missionId, string VDate, string Vhours, string Vmins, string VNote)
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("userid"));
            var hrs = Convert.ToInt32(Vhours);
            var mins = Convert.ToInt32(Vmins);



            if (timesheetId == 0)
            {
                Timesheet ts = new Timesheet();

                ts.MissionId = Convert.ToInt64(missionId);
                ts.DateVolunteered = Convert.ToDateTime(VDate);
                ts.TimesheetTime = new(hrs, mins, 0);
                ts.Notes = VNote;
                ts.UserId = userId;

                _ciplatformDbContext.Timesheets.Add(ts);
                _ciplatformDbContext.SaveChanges();
            }
            else
            {
                var Timesheet = _ciplatformDbContext.Timesheets.FirstOrDefault(ts => ts.TimesheetId == timesheetId);

                Timesheet.MissionId = Convert.ToInt64(missionId);
                Timesheet.DateVolunteered = Convert.ToDateTime(VDate);
                Timesheet.TimesheetTime = new(hrs, mins, 0);
                Timesheet.Notes = VNote;
                Timesheet.UserId = userId;

                TempData["success"] = "Timesheet updated successfully";

                _ciplatformDbContext.Timesheets.Update(Timesheet);
                _ciplatformDbContext.SaveChanges();
            }
            return true;
        }

        [HttpPost]
        public bool TimesheetForGoal(long timesheetId, string missionId, string gAction, string gDate, string gNotes)
        {

            var userId = Convert.ToInt64(HttpContext.Session.GetString("userid"));

            if (timesheetId == 0)
            {
                Timesheet ts = new Timesheet();

                ts.MissionId = Convert.ToInt64(missionId);
                ts.DateVolunteered = Convert.ToDateTime(gDate);
                ts.Action = Convert.ToInt32(gAction);
                ts.UserId = userId;
                ts.Notes = gNotes;

                _ciplatformDbContext.Timesheets.Add(ts);
                _ciplatformDbContext.SaveChanges();
            }
            else
            {
                var Timesheet = _ciplatformDbContext.Timesheets.FirstOrDefault(ts => ts.TimesheetId == timesheetId);

                Timesheet.TimesheetId = timesheetId;
                Timesheet.MissionId = Convert.ToInt64(missionId);
                Timesheet.Action = Convert.ToInt32(gAction);
                Timesheet.UserId = userId;
                Timesheet.Notes = gNotes;

                TempData["success"] = "Timesheet updated successfully";

                _ciplatformDbContext.Timesheets.Update(Timesheet);
                _ciplatformDbContext.SaveChanges();

            }
            return true;

        }


        public JsonResult EditTimeTs(long id)
        {
            var Ts = _ciplatformDbContext.Timesheets.FirstOrDefault(ts => ts.TimesheetId == id);
            var mission = _ciplatformDbContext.Missions.FirstOrDefault(m => m.MissionId == Ts.MissionId);

            var timesheetVM = new TimesheetVM
            {
                TimeSheetId = id,
                MissionId = mission.MissionId,
                MissionName = mission.Title,
                StartDate = DateTime.Parse(mission.StartDate?.ToShortDateString()),
                EndDate = DateTime.Parse(mission.EndDate?.ToShortDateString()),
                VolunteerDate = Ts.DateVolunteered,
                VolunteerHrs = Ts.TimesheetTime.GetValueOrDefault().Hour,
                VolunteerMins = Ts.TimesheetTime.GetValueOrDefault().Minute,
                Notes = Ts.Notes,
            };
            return new JsonResult(timesheetVM);
        }




        public JsonResult EditGoalTs(long id)
        {

            var Ts = _ciplatformDbContext.Timesheets.FirstOrDefault(ts => ts.TimesheetId == id);
            var mission = _ciplatformDbContext.Missions.FirstOrDefault(m => m.MissionId == Ts.MissionId);

            var timesheetVM = new TimesheetVM
            {
                TimeSheetId = id,
                MissionId = mission.MissionId,
                MissionName = mission.Title,
                StartDate = DateTime.Parse(mission.StartDate?.ToString("dd-MM-yyyy")),
                EndDate = DateTime.Parse(mission.EndDate?.ToString("dd-MM-yyyy")),
                VolunteerDate = Ts.DateVolunteered,
                GoalAction = Ts.Action,
                Notes = Ts.Notes,
            };
            return new JsonResult(timesheetVM);
        }





        [HttpDelete]
        public bool DeleteTimesheet(long id)
        {
            var ts = _ciplatformDbContext.Timesheets.FirstOrDefault(ts => ts.TimesheetId == id);

            TempData["success"] = "Record deleted successfully.";

            _ciplatformDbContext.Timesheets.Remove(ts);
            _ciplatformDbContext.SaveChanges();


            return true;
        }


        public IActionResult Admin_Mission(AdminMissionVM adminmissionvm, int countryid, int missionid)
        {
            var missionlist = _ciplatformDbContext.Missions.Where(a => a.DeletedAt == null).ToList();
            ViewBag.missionlist = missionlist;
            var countrylist = _ciplatformDbContext.Countries.ToList();
            ViewBag.countrylist = countrylist;

            var themelist = _homeRepository.GetMissionThemes();
            ViewBag.themelist = themelist;


            if (missionid != 0)
            {
                _homeRepository.deletemission(missionid);


            }

            var skillls = _homeRepository.GetSkills();
            ViewBag.skills = skillls;

            if (countryid != 0)
            {
                var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
                ViewBag.citylist = citylist;
            }
            else
            {

                var citylist = _ciplatformDbContext.Cities.ToList();
                ViewBag.citylist = citylist;
            }

            if (adminmissionvm.Title != null)
            {
                var missionobj = new Mission
                {
                    Title = adminmissionvm.Title,
                    StartDate = adminmissionvm.StartDate,
                    EndDate = adminmissionvm.EndDate,
                    MissionType = adminmissionvm.MissionType,

                };
                _ciplatformDbContext.Missions.Add(missionobj);
                _ciplatformDbContext.SaveChanges();
            }

            return View();
        }









        public IActionResult AdminAddMission(IFormFileCollection files, int countryid, AdminMissionVM adminmission)

        {

            var md = _ciplatformDbContext.MissionMedia.Where(a => a.MediaName == "trial").FirstOrDefault();


            var missionlist = _homeRepository.GetMission();
            ViewBag.missionlist = missionlist;
            var countrylist = _ciplatformDbContext.Countries.ToList();
            ViewBag.countrylist = countrylist;

            var themelist = _homeRepository.GetMissionThemes();
            ViewBag.themelist = themelist;


            var skillls = _homeRepository.GetSkills();
            ViewBag.skills = skillls;

            if (countryid != 0)
            {
                var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
                ViewBag.citylist = citylist;
            }
            else
            {

                var citylist = _ciplatformDbContext.Cities.ToList();
                ViewBag.citylist = citylist;
            }



            if (adminmission.Title != null && adminmission.StartDate > DateTime.Today && adminmission.EndDate > DateTime.Today)
            {
                var file = Request.Form["files"];
                var city = _ciplatformDbContext.Cities.FirstOrDefault(a => a.Name == adminmission.CityId);

                var ciId = city.CityId;
                var cntryid = city.CountryId;
                var missionobj = new Mission
                {
                    Title = adminmission.Title,
                    OrganizationName = adminmission.OrganizationName,
                    OrganizationDetail = adminmission.OrganizationDetail,
                    StartDate = adminmission.StartDate,
                    EndDate = adminmission.EndDate,
                    Description = adminmission.Description,
                    ShortDescription = adminmission.ShortDescription,
                    ThemeId = Convert.ToInt32(adminmission.ThemeId),
                    Availability = adminmission.Availability,
                    MissionAvailability = adminmission.MissionAvailability,
                    CityId = ciId,
                    MissionType = adminmission.MissionType,
                    CountryId = cntryid
                };

                _ciplatformDbContext.Missions.Add(missionobj);
                _ciplatformDbContext.SaveChanges();

                foreach (var msnskill in adminmission.SelectedValues)
                {

                    var missionskill = new MissionSkill
                    {
                        MissionId = missionobj.MissionId,
                        SkillId = Convert.ToInt32(msnskill),
                        CreatedAt = DateTime.Now


                    };
                    _ciplatformDbContext.MissionSkills.Add(missionskill);
                    _ciplatformDbContext.SaveChanges();


                }


                foreach (var filem in adminmission.File)
                {


                    if (filem != null && filem.Length > 0)
                    {
                        byte[] imageBytes = null;

                        using (var stream = new MemoryStream())
                        {
                            filem.CopyTo(stream);
                            imageBytes = stream.ToArray();
                        }

                        string base64String = Convert.ToBase64String(imageBytes);


                        var missionid = missionobj.MissionId;

                        var missionmedia = new MissionMedium
                        {

                            MediaName = filem.FileName,
                            MediaType = filem.ContentType,
                            MediaPath = base64String,
                            MissionId = missionid,
                            CreatedAt = DateTime.Now

                        };
                        _ciplatformDbContext.MissionMedia.Add(missionmedia);
                        _ciplatformDbContext.SaveChanges();

                    }


                }

                foreach (var docfile in adminmission.DocumentFile)
                {


                    if (docfile != null && docfile.Length > 0)
                    {
                        byte[] imageBytes = null;

                        using (var stream = new MemoryStream())
                        {
                            docfile.CopyTo(stream);
                            imageBytes = stream.ToArray();
                        }

                        string documentString = Convert.ToBase64String(imageBytes);


                        var missionid = missionobj.MissionId;
                        var doc = new MissionDocument
                        {
                            DocumentPath = documentString,
                            MissionId = missionid,
                            DocumentName = docfile.FileName,
                            DocumentType = docfile.ContentType,
                            CreatedAt = DateTime.Now

                        };
                        _ciplatformDbContext.MissionDocuments.Add(doc);
                        _ciplatformDbContext.SaveChanges();
                    }
                }





                ViewBag.Status = 1;
                TempData["DataAdd"] = "Mission added successfully";
            }

            return View();
        }


        public IActionResult AdminEditMission(long id, int countryid, AdminMissionVM adminmission, int missionsvalue)
        {


            if (id == 0)
            {
                id = adminmission.MissionId;
            }
            List<string> mediaList = new List<string>();
            var missionmedia = _homeRepository.GetMissionMediaJoin(id);




            if (missionmedia.Any())
            {

                foreach (var m in missionmedia)
                {
                    var a = "data:image/png;base64," + m.MediaPath;
                    mediaList.Add(a);


                }
            }


            ViewBag.mediaList = mediaList;
            var countrylist = _ciplatformDbContext.Countries.ToList();
            ViewBag.countrylist = countrylist;

            var themelist = _homeRepository.GetMissionThemes();
            ViewBag.themelist = themelist;


            var skillls = _homeRepository.GetSkills();
            ViewBag.skills = skillls;

            if (countryid != 0)
            {
                var citylist = _ciplatformDbContext.Cities.Where(a => a.CountryId == countryid).ToList();
                ViewBag.citylist = citylist;
            }
            else
            {

                var citylist = _ciplatformDbContext.Cities.ToList();
                ViewBag.citylist = citylist;
            }

            var missionlist = _ciplatformDbContext.Missions.Where(a => a.MissionId == id).FirstOrDefault();
            ViewBag.missionlist = missionlist;

            AdminMissionVM modelvm = new AdminMissionVM();
            modelvm.Title = missionlist.Title;
            modelvm.OrganizationDetail = missionlist.OrganizationDetail;
            modelvm.CityId = missionlist.CityId.ToString();
            modelvm.Availability = missionlist.Availability;
            modelvm.MissionAvailability = missionlist.MissionAvailability;
            modelvm.Status = missionlist.Status;
            modelvm.OrganizationName = missionlist.OrganizationName;
            modelvm.CountryId = missionlist.CountryId;
            modelvm.MissionId = missionlist.MissionId;
            modelvm.Description = missionlist.Description;
            modelvm.ShortDescription = missionlist.ShortDescription;





            //EDIT Mission


            if (adminmission.Title != null)
            {
                var city = _ciplatformDbContext.Cities.FirstOrDefault(a => a.Name == adminmission.CityId);

                var ciId = city.CityId;
                var cntryid = city.CountryId;
                var missiondata = _ciplatformDbContext.Missions.Where(y => y.MissionId == id).ToList();
                var query = from m in missiondata select m;
                foreach (Mission m in query)
                {
                    m.Title = adminmission.Title;
                    m.OrganizationName = adminmission.OrganizationName;
                    m.OrganizationDetail = adminmission.OrganizationDetail;
                    m.StartDate = adminmission.StartDate;
                    m.EndDate = adminmission.EndDate;
                    m.Description = adminmission.Description;
                    m.ShortDescription = adminmission.ShortDescription;
                    m.ThemeId = Convert.ToInt32(adminmission.ThemeId);
                    m.Availability = adminmission.Availability;
                    m.MissionAvailability = adminmission.MissionAvailability;
                    m.MissionType = adminmission.MissionType;
                    m.CountryId = cntryid;
                    m.CityId = city.CityId;

                    _ciplatformDbContext.SaveChanges();
                };


                if (adminmission.File.Count == 0)
                {
                    var missionimg = _ciplatformDbContext.MissionMedia.Where(a => a.MissionId == id).ToList();
                    if (missionimg != null)
                    {
                        foreach (var img in missionimg)
                        {
                            _ciplatformDbContext.MissionMedia.Remove(img);
                            _ciplatformDbContext.SaveChanges();
                        }


                    }
                }
            }

            return View(modelvm);


        }




        [HttpPost]
        public IActionResult UploadProfilePicture(IFormFile profilePicture)
        {
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);

            long uid = Convert.ToInt64(ids);

            if (profilePicture != null && profilePicture.Length > 0)
            {
                var user = _ciplatformDbContext.Users.FirstOrDefault(u => (u.UserId == ids)); // replace userId with the actual user ID
                if (user != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        profilePicture.CopyToAsync(memoryStream);
                        string avatar = Convert.ToBase64String(memoryStream.ToArray());


                        // Add avatar to usr tbl

                        user.Avatar = avatar;
                        _ciplatformDbContext.Users.Update(user);
                        _ciplatformDbContext.SaveChanges();

                    }

                }
            }


            return RedirectToAction("EditProfile", "Home");
        }

        public IActionResult Admin_Banner(int bnrid)
        {

            var bannerlst = _ciplatformDbContext.Banners.ToList();
            ViewBag.bannerlst = bannerlst;


            // delete data

            var bannerdata = _ciplatformDbContext.Banners.Where(u => u.BannerId == bnrid).ToList();

            var bnr = _ciplatformDbContext.Banners.FirstOrDefault(bannerdata => bannerdata.BannerId == bnrid);
            ModelState.Clear();
            if (bnr != null)
            {
                bnr.DeletedAt = DateTime.Now;
                _ciplatformDbContext.SaveChanges();
            }

            return View();
        }


        public IActionResult Admin_AddBanner(IFormFile files, AdminBannerVM _adBaneer)
        {

            var filem = _adBaneer.formFile;


            if (filem != null && filem.Length > 0)
            {
                byte[] imageBytes = null;

                using (var stream = new MemoryStream())
                {
                    filem.CopyTo(stream);
                    imageBytes = stream.ToArray();
                }

                string base64String = Convert.ToBase64String(imageBytes);


                var banner = new Banner
                {


                    Image = base64String,
                    Text = _adBaneer.Text,
                    SortOrder = _adBaneer.SortOrder,
                    CreatedAt = DateTime.Now,
                    ImageName = filem.FileName

                };

                _ciplatformDbContext.Banners.Add(banner);
                _ciplatformDbContext.SaveChanges();

            }




            TempData["DataSaved"] = "Banner added successfully";


            return RedirectToAction("Admin_Banner", "Home");
        }

        public IActionResult EditBanner(int bnrid, IFormFile ImageEdit)
        {
            var data = _ciplatformDbContext.Banners.Where(a => a.BannerId == bnrid).ToList().FirstOrDefault();
            ViewBag.bannerimg = data.Image;
            if (data != null)
            {

                var fetchBannerdetails = new Banner
                {

                    Image = data.Image,
                    Text = data.Text,
                    SortOrder = data.SortOrder,
                    BannerId = bnrid


                };


                var bannerimg = _ciplatformDbContext.Banners.Where(a => a.BannerId == bnrid).FirstOrDefault();
                ViewBag.bannerimg = bannerimg.Image;

                TempData["EditedData"] = "Banner updated successfully";


                return Json(fetchBannerdetails);



            }
            else
            {
                return View();
            }
        }


        public IActionResult SaveBannerData(int bnrid, string image, string text, int sortOrder)
        {

            var bannerData = _ciplatformDbContext.Banners.Where(y => y.BannerId == bnrid).ToList();

            var query = from r in bannerData select r;

            if (text != null && sortOrder != 0)
            {
                foreach (Banner r in query)
                {

                    r.Image = image;
                    r.Text = text;
                    r.SortOrder = sortOrder;
                    r.BannerId = bnrid;

                }
            }
            _ciplatformDbContext.SaveChanges();
            TempData["DataSavedMessage"] = "Data has been saved successfully!";
            return RedirectToAction("Admin_Banner", "Home");
        }

        public IActionResult Landing()
        {
            return View();
        }
    }


}
#endregion
