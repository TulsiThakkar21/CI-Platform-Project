using CIPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIPlatform.Controllers
{
    public class HomeController : Controller
    {
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
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
            var status = _ciplatformDbContext.Users.Where(u=>u.Email==_loginModel.LoginId && u.Password==_loginModel.Password).FirstOrDefault();
            
            if(status == null)
            {
                ViewBag.LoginStatus = 0;
            }
            else
            {
                return RedirectToAction("PlatformLandingPage", "Home");
            }
            
            return View(_loginModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LostPass()
        {
            return View();
        }

        public IActionResult ResetPass()
        {
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
                    PhoneNumber = _registrationModel.PhoneNumber,
                    Email = _registrationModel.Email,
                    Password = _registrationModel.Password,
                    CityId = _registrationModel.CityId,
                    CountryId = _registrationModel.CountryId

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

        public IActionResult PlatformLandingPage()
        {
            return View();
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