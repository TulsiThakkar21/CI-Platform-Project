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

            //if (status == null)
            //{
            //    ViewBag.LoginStatus = 0;
            //}
            //else
            //{

            //    string UserIDf = user.UserId.ToString();

            //    HttpContext.Session.SetString("userid", UserIDf);
            //    var abc = HttpContext.Session.GetString("userid");

            //    long abcd = Convert.ToInt64(abc);
            //    // var loginuser = _ciplatformDbContext.Users.FirstOrDefault(x => (x.UserId == abcd));
            //    //var loginuser = _ciplatformDbContext.Users.Where(u => (u.UserId == abcd)).FirstOrDefault();
            //    var loginuser = _ciplatformDbContext.Users.FirstOrDefault(x => (x.UserId == abcd));
            //    if (loginuser != null)
            //    {
            //        var loginfname = loginuser.FirstName;
            //        var loginlname = loginuser.LastName;

            //        TempData["fullname"] = loginfname + loginlname;


            //    }


            //    return RedirectToAction("PlatformLandingPage", "Home", new { @Id = user.UserId });
            //}


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


                }


                return RedirectToAction("PlatformLandingPage", "Home", new { @Id = status.UserId });

            }
            else
            {

                return RedirectToAction("Login", "Home");
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
                var userData = new User()
                {
                    FirstName = _registrationModel.FirstName,
                    LastName = _registrationModel.LastName,
                    Email = _registrationModel.Email,
                    Password = _registrationModel.Password,
                    CityId = _registrationModel.CityId,
                    CountryId = _registrationModel.CountryId
                    //PhoneNumber = _registrationModel.PhoneNumber.ToString()

            };
                _ciplatformDbContext.Users.Add(userData);
                _ciplatformDbContext.SaveChanges();
                ViewBag.Status = 1;
            }
            catch
            {
                ViewBag.Status = 0;
            }


            return View();
        }

        [HttpGet]
        public IActionResult PlatformLandingPage(string searching, LandingAllModels landingAllModels, string filter, string country, string city)
        {

            //var missionxx = _ciplatformDbContext.Missions.ToList();
            var missionxx = _ciplatformDbContext.Missions.Where(k => k.Title.Contains(searching) || searching == null).ToList();
            
            if (missionxx.Count == 0)
            {
                ViewBag.SearchStatus = 0;
            }
            //var missionthemexx = _ciplatformDbContext.MissionThemes.ToList();
            var missionthemexx = _ciplatformDbContext.MissionThemes.Where(i => i.Title.Contains(filter) || filter == null).ToList();



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
                         join cnt in countryx on m.CountryId equals cnt.CountryId where m.CountryId == cnt.CountryId
                         join cit in cityx on m.CityId equals cit.CityId where m.CityId == cit.CityId
                         where m.CountryId == cnt.CountryId
                         
                         select new
                         {

                             m,
                             mt.Title,
                             mt.MissionThemeId,
                             cnt.Name,
                             City = cit.Name
                         };

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




            //if (!string.IsNullOrEmpty(filter))
            //{ 
            //    item = _ciplatformDbContext.MissionThemes.Where(i => i.Title.Contains(filter) || filter == null).ToList();

            //}


           

            return View();
            //ViewBag.FilterValue = filter;
          
              
        }



        public IActionResult VolunteeringMission()
        {
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