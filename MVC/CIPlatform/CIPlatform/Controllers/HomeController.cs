using CIPlatform.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using CIPlatform.Models.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CIPlatform.Controllers
{
    public class HomeController : Controller
    {
        CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
           // CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
            var status = _ciplatformDbContext.Users.Where(u => u.Email == _loginModel.LoginId && u.Password == _loginModel.Password).FirstOrDefault();

           

            if (status != null)
            {
               
                string UserIDf = status.UserId.ToString();

                HttpContext.Session.SetString("userid", UserIDf);
                var abc = HttpContext.Session.GetString("userid");

                long abcd = Convert.ToInt64(abc);
                var loginuser = _ciplatformDbContext.Users.FirstOrDefault(x => (x.UserId == abcd));
                if (loginuser != null)
                {
                    var loginfname = loginuser.FirstName;
                    var loginlname = loginuser.LastName;

                    TempData["fullname"] = loginfname + loginlname;

                    ViewBag.LoginStatus = 1;
                }


                return RedirectToAction("PlatformLandingPage", "Home", new { @Id = status.UserId });

            }
            else
            {
                ViewBag.LoginStatus = 0;
               // return RedirectToAction("Login", "Home");
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

                // var lnkHref = "<a asp-controller='Home'  asp-action='ResetPass' " + Url.Action("ResetPass/" + passwordReset.Token, "Home", new { email = passwordReset.Email, code = passwordReset.Token }, "http") +  "'>Reset Password</a>";

                //var lnkHref = Url.ActionLink("ResetPass", "Home", new { email = passwordReset.Email, token = passwordReset.Token }, "http") + "'>Reset Password</a>";

                var lnkHref = Url.ActionLink("ResetPass", "Home", new { id = generated_token });

                //var lnkHref = "<a href=''" + generated_token + "></a>";

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
            //if (modellogin.Email == passwordReset.Email && path==passwordReset.Token) {
            // modellogin.Password = passwd;

            //}
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
                //var status = _ciplatformDbContext.Users.Where(u => u.Email == _registrationModel.Email && u.PhoneNumber == _registrationModel.PhoneNumber.ToString()).FirstOrDefault();
                var flag = _ciplatformDbContext.Users.Where(a => a.Email == _registrationModel.Email && a.PhoneNumber == _registrationModel.PhoneNumber.ToString());
                //if()
                if (flag.Count()==0)
                {
                    var userData = new User()
                    {
                        FirstName = _registrationModel.FirstName,
                        LastName = _registrationModel.LastName,
                        Email = _registrationModel.Email,
                        Password = _registrationModel.Password,
                        CityId = _registrationModel.CityId,
                        CountryId = _registrationModel.CountryId,
                        PhoneNumber = _registrationModel.PhoneNumber.ToString()

                    };
                    _ciplatformDbContext.Users.Add(userData);
                    _ciplatformDbContext.SaveChanges();
                    ViewBag.Status = 1;
                }

                else { 
                ViewBag.Status = 0;
                
                }
               
            }
            catch
            {
                ViewBag.Status = 0;
            }


            return View();
        }

        //[HttpGet]
        //public IActionResult PlatformLandingPage(string searching, LandingAllModels landingAllModels, string filter, string country, string city, string sortOrder = "", int page=1, int pageSize=6)
        //{
        //    var userId = HttpContext.Session.GetString("userid");
        //    var userids = Convert.ToInt32(userId);
        //    ViewBag.userids = userids;
        //    var Ratingdata = _ciplatformDbContext.MissionRatings.Where(a => a.UserId == userids).ToList();
        //    landingAllModels.missionRatings = Ratingdata;
        //    //for add to fav
        //    var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        //    ViewBag.ids = Convert.ToInt32(ids);
        //    var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
        //    ViewBag.favlist = favlist;
        //    //till here

        //    var ifexist = HttpContext.Session.GetString("userid");
        //    if (ifexist == null)

        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    //var missionxx = _ciplatformDbContext.Missions.ToList();
        //    var missionxx = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();

        //    if (missionxx.Count == 0)
        //    {
        //        ViewBag.SearchStatus = 0;
        //    }
        //    //var missionthemexx = _ciplatformDbContext.MissionThemes.ToList();
        //    var missionthemexx = _ciplatformDbContext.MissionThemes.Where(i => i.Title.Contains(filter) || filter == null).ToList();

        //    //var items = from i in _ciplatformDbContext.Missions
        //    //          select new 
        //    //          {
        //    //              Id = i.MissionId,
        //    //              Name = i.Title,

        //    //          };

        //    ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
        //    ViewBag.DateSortParamAsc = sortOrder == "Date" ? "date_desc" : "Date";
        //    ViewBag.LowestSeats = sortOrder == "LowSeats" ? "HighSeats" : "LowSeats";
        //    ViewBag.HighestSeats = sortOrder == "HighSeats" ? "LowSeats" : "HighSeats";
        //    ViewBag.DeadlineNear = sortOrder == "near" ? "far" : "near";
        //    ViewBag.DeadlineFar = sortOrder == "far" ? "near" : "far";

        //    switch (sortOrder)
        //    {

        //        case "Date":
        //            missionxx = missionxx.OrderBy(a => a.StartDate).ToList();
        //            break;
        //        case "date_desc":
        //            missionxx = missionxx.OrderByDescending(a => a.StartDate).ToList();
        //            break;
        //        case "LowSeats":
        //            missionxx = missionxx.OrderBy(a => a.Availability).ToList();
        //            break;
        //        case "HighSeats":
        //            missionxx = missionxx.OrderByDescending(a => a.Availability).ToList();
        //            break;
        //        case "near":
        //            missionxx = missionxx.OrderBy(a => a.EndDate).ToList();
        //            break;
        //        case "far":
        //            missionxx = missionxx.OrderByDescending(a => a.EndDate).ToList();
        //            break;
        //        default:
        //            missionxx= missionxx.ToList();
        //            break;
        //    }


        //    //var countryx = _ciplatformDbContext.Countries.ToList();
        //    //var filteredCountries = new List<Country>();
        //    //foreach (var countryy in countryx)
        //    //{
        //    //    var filtered = _ciplatformDbContext.Countries
        //    //        .Where(c => c.Name.Contains(countryy.Name) || countryy.Name == null)
        //    //        .ToList();
        //    //    filteredCountries.AddRange(filtered);
        //    //}
        //    //ViewBag.Country = filteredCountries;


        //    var countryx = _ciplatformDbContext.Countries.Where(c => c.Name.Contains(country) || country == null).ToList();
        //    ViewBag.Country = countryx;

        //    var cityx = _ciplatformDbContext.Cities.Where(ci => ci.Name.Contains(city) || city == null).ToList();
        //    ViewBag.Cities = cityx;


        //    var result = from m in missionxx
        //                 join mt in missionthemexx on m.ThemeId equals mt.MissionThemeId
        //                 where m.ThemeId == mt.MissionThemeId
        //                 join cnt in countryx on m.CountryId equals cnt.CountryId where m.CountryId == cnt.CountryId
        //                 join cit in cityx on m.CityId equals cit.CityId where m.CityId == cit.CityId
        //                 where m.CountryId == cnt.CountryId


        //                 select new
        //                 {

        //                     m,
        //                     date= m.StartDate,
        //                     m.MissionId,
        //                     mt.Title,
        //                     mt.MissionThemeId,
        //                     cnt.Name,
        //                     City = cit.Name
        //                 };


        //    @ViewData["page"] = page;
        //    ViewData["PageSize"] = pageSize;
        //    ViewData["TotalPages"] = (int)Math.Ceiling((decimal)missionxx.Count / pageSize);
        //    ViewBag.outputsf = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();



        //    ViewBag.Result = result;

        //    var missionx = _ciplatformDbContext.Missions.ToList();
        //    ViewBag.Missions = missionx;





        //    //var countryx = _ciplatformDbContext.Countries.Where(c => c.Name.Contains(countries) || countries == null).ToList();
        //    //ViewBag.Country = countryx;


        //    // var selectedCountries = landingAllModels.SelectedCountries ?? new List<string>();






        //    var themex = _ciplatformDbContext.MissionThemes.ToList();
        //    ViewBag.MissionThemes = themex;



        //    var skillx = _ciplatformDbContext.MissionSkills.ToList();
        //    ViewBag.MissionSkills = skillx;




        //    //for sort by


        //    //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

        //    //var missions = from m in _ciplatformDbContext.Missions
        //    //               select m;

        //    //switch (sortOrder)
        //    //{
        //    //    case "name_desc":
        //    //        missions = missions.OrderByDescending(m => m.Title);
        //    //        break;
        //    //    case "Date":
        //    //        missions = missions.OrderBy(m => m.CreatedAt);
        //    //        break;
        //    //    case "date_desc":
        //    //        missions = missions.OrderByDescending(m => m.CreatedAt);
        //    //        break;
        //    //    default:
        //    //        missions = missions.OrderBy(m => m.Title);
        //    //        break;
        //    //}
        //    // IQueryable<Mission> mission_get = this._ciplatformDbContext.Missions;
        //    // mission_get = _ciplatformDbContext.Missions.Where(x => x.MissionId == true);



        //    return View();



        //}




        //[HttpPost]
        //public IActionResult AddFavorite(int itemId, int userId)
        //{
        //    var item = _ciplatformDbContext.FavoriteMissions.FirstOrDefault(i => i.FavouriteMissionId == itemId);
        //    var user = _ciplatformDbContext.Users.Include(u => u.FavoriteMissions).FirstOrDefault(u => u.UserId == userId);

        //    if (item == null || user == null)
        //    {
        //        return NotFound();
        //    }

        //    user.FavoriteMissions.Add(item);
        //    _ciplatformDbContext.SaveChanges();

        //    return Ok();
        //}
        //[HttpGet]
        //public IActionResult Add() {

        //    return RedirectToAction("PlatformLandingPage", "Home");


        //}





        //======================================================TRIAL=======================================================

        [HttpGet]
        public IActionResult PlatformLandingPage(string searching, LandingAllModels landingAllModels, string filter, string country, string city, string sortOrder = "", int page = 1, int pageSize = 6)
        {
            var userId = HttpContext.Session.GetString("userid");
            var userids = Convert.ToInt32(userId);
            ViewBag.userids = userids;
            var Ratingdata = _ciplatformDbContext.MissionRatings.Where(a => a.UserId == userids).ToList();
            ViewBag.Ratingdata = Ratingdata;
            //for add to fav
            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
            ViewBag.favlist = favlist;
            //till here


            



            var ifexist = HttpContext.Session.GetString("userid");
            if (ifexist == null)

            {
                return RedirectToAction("Index", "Home");
            }

            //var missionxx = _ciplatformDbContext.Missions.ToList();
            var missionxx = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();

            if (missionxx.Count == 0)
            {
                ViewBag.SearchStatus = 0;
            }
            //var missionthemexx = _ciplatformDbContext.MissionThemes.ToList();
            var missionthemexx = _ciplatformDbContext.MissionThemes.Where(i => i.Title.Contains(filter) || filter == null).ToList();

            //var items = from i in _ciplatformDbContext.Missions
            //          select new 
            //          {
            //              Id = i.MissionId,
            //              Name = i.Title,

            //          };

            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.DateSortParamAsc = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LowestSeats = sortOrder == "LowSeats" ? "HighSeats" : "LowSeats";
            ViewBag.HighestSeats = sortOrder == "HighSeats" ? "LowSeats" : "HighSeats";
            ViewBag.DeadlineNear = sortOrder == "near" ? "far" : "near";
            ViewBag.DeadlineFar = sortOrder == "far" ? "near" : "far";

            switch (sortOrder)
            {

                case "Date":
                    missionxx = missionxx.OrderBy(a => a.StartDate).ToList();
                    break;
                case "date_desc":
                    missionxx = missionxx.OrderByDescending(a => a.StartDate).ToList();
                    break;
                case "LowSeats":
                    missionxx = missionxx.OrderBy(a => a.Availability).ToList();
                    break;
                case "HighSeats":
                    missionxx = missionxx.OrderByDescending(a => a.Availability).ToList();
                    break;
                case "near":
                    missionxx = missionxx.OrderBy(a => a.EndDate).ToList();
                    break;
                case "far":
                    missionxx = missionxx.OrderByDescending(a => a.EndDate).ToList();
                    break;
                default:
                    missionxx = missionxx.ToList();
                    break;
            }


            //var countryx = _ciplatformDbContext.Countries.ToList();
            //var filteredCountries = new List<Country>();
            //foreach (var countryy in countryx)
            //{
            //    var filtered = _ciplatformDbContext.Countries
            //        .Where(c => c.Name.Contains(countryy.Name) || countryy.Name == null)
            //        .ToList();
            //    filteredCountries.AddRange(filtered);
            //}
            //ViewBag.Country = filteredCountries;


            var countryx = _ciplatformDbContext.Countries.Where(c => c.Name.Contains(country) || country == null).ToList();
            ViewBag.Country = countryx;

            var cityx = _ciplatformDbContext.Cities.Where(ci => ci.Name.Contains(city) || city == null).ToList();
            ViewBag.Cities = cityx;


            var result = from m in missionxx
                         join mt in missionthemexx on m.ThemeId equals mt.MissionThemeId
                         where m.ThemeId == mt.MissionThemeId
                         join cnt in countryx on m.CountryId equals cnt.CountryId
                         where m.CountryId == cnt.CountryId
                         join cit in cityx on m.CityId equals cit.CityId
                         where m.CityId == cit.CityId
                         where m.CountryId == cnt.CountryId


                         select new
                         {

                             m,
                             date = m.StartDate,
                             m.MissionId,
                             mt.Title,
                             mt.MissionThemeId,
                             cnt.Name,
                             City = cit.Name
                         };


            @ViewData["page"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((decimal)missionxx.Count / pageSize);
            ViewBag.outputsf = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();



            ViewBag.Result = result;

            var missionx = _ciplatformDbContext.Missions.ToList();
            ViewBag.Missions = missionx;





            //var countryx = _ciplatformDbContext.Countries.Where(c => c.Name.Contains(countries) || countries == null).ToList();
            //ViewBag.Country = countryx;


            // var selectedCountries = landingAllModels.SelectedCountries ?? new List<string>();






            var themex = _ciplatformDbContext.MissionThemes.ToList();
            ViewBag.MissionThemes = themex;



            var skillx = _ciplatformDbContext.MissionSkills.ToList();
            ViewBag.MissionSkills = skillx;




            //for sort by


            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //var missions = from m in _ciplatformDbContext.Missions
            //               select m;

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        missions = missions.OrderByDescending(m => m.Title);
            //        break;
            //    case "Date":
            //        missions = missions.OrderBy(m => m.CreatedAt);
            //        break;
            //    case "date_desc":
            //        missions = missions.OrderByDescending(m => m.CreatedAt);
            //        break;
            //    default:
            //        missions = missions.OrderBy(m => m.Title);
            //        break;
            //}
            // IQueryable<Mission> mission_get = this._ciplatformDbContext.Missions;
            // mission_get = _ciplatformDbContext.Missions.Where(x => x.MissionId == true);



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
                //var f = missionId;
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

        //[HttpPost]
        //public IActionResult Recommend(string[] selectedValues)
        //{
        //    var userslist = _ciplatformDbContext.Users.ToList();
        //    ViewBag.userslist = userslist;


        //    return PartialView("Recommend", userslist);

        //    //return RedirectToAction("VolunteeringMission", "Home");
        //}


        [HttpPost]
        public IActionResult Recommendedto(string checkbox_value, int MissionId)
        {

            //string[] authorsList = checkbox_value.Split(", ");

            //var a = authorsList[0];
            string[] authorsList = { "," };
            int count = 30;
            string[] author = checkbox_value.Split(",", count, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in author)
            {
                var k = s;
                Console.WriteLine(k);



                try
                {
                    MailMessage newMail = new MailMessage();
                    SmtpClient client = new SmtpClient("smtp.gmail.com");
                    newMail.From = new MailAddress("tulsithakkar21@gmail.com", "CI PLATFORM");
                    newMail.To.Add(s);// declare the email subject
                    newMail.Subject = "My First Email";
                    newMail.IsBodyHtml = true;
                    var lnkHref = Url.ActionLink("Volunteeringpage", "Home", new { id = MissionId });
                    newMail.Body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;
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







        [HttpGet]
        public IActionResult VolunteeringMission(int id)
        {

            var userslist = _ciplatformDbContext.Users.ToList();
            ViewBag.userslist = userslist;



            var ids = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.ids = Convert.ToInt32(ids);
            var favlist = _ciplatformDbContext.FavoriteMissions.Where(a => a.UserId == ids).ToList();
            ViewBag.favlist = favlist;
            var Ratingdata = _ciplatformDbContext.MissionRatings.Where(a => a.UserId == ids).ToList();
            ViewBag.Ratingdata = Ratingdata;
            var b = id;
            var missionthemelist = _ciplatformDbContext.MissionThemes.ToList();
            var citylist = _ciplatformDbContext.Cities.ToList();
            var countrylist = _ciplatformDbContext.Countries.ToList();
            var specificmission = _ciplatformDbContext.Missions.Where(a => a.MissionId == b).ToList();

            var result = from m in specificmission
                         join mt in missionthemelist on m.ThemeId equals mt.MissionThemeId
                         where m.ThemeId == mt.MissionThemeId
                         join cty in citylist on m.CityId equals cty.CityId
                         where cty.CityId == m.CityId
                         select new
                         {

                             m,
                             mt.Title,
                             mt.MissionThemeId,
                             cty.CityId,
                             cty.Name,

                         };
            ViewBag.Result = result;

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




            return View();
        }




     

        public IActionResult NoMissionFound()
        {
            return View();
        }
        public IActionResult StoryListing()
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