using CIPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;


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

        [HttpGet]
        public IActionResult LostPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LostPass(ForgotPasswordModel _forgotPasswordModel, PasswordReset passwordReset)
        {
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();

            try
            {
                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress("tulsithakkar21@gmail.com", "CI PLATFORM");

                newMail.To.Add("tulsithakkar21@gmail.com");// declare the email subject

                newMail.Subject = "My First Email"; // use HTML for the email body

                newMail.IsBodyHtml = true;

                // var lnkHref = "<a asp-controller='Home'  asp-action='ResetPass' " + Url.Action("ResetPass/" + passwordReset.Token, "Home", new { email = passwordReset.Email, code = passwordReset.Token }, "http") +  "'>Reset Password</a>";

                //var lnkHref = Url.ActionLink("ResetPass", "Home", new { email = passwordReset.Email, token = passwordReset.Token }, "http") + "'>Reset Password</a>";

                var lnkHref =Url.ActionLink("ResetPass", passwordReset.Token,"Home");

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
                        Email = passwordReset.Email,
                        Token = passwordReset.Token
                    };
                    _ciplatformDbContext.PasswordReset.Add(passdata);
                    _ciplatformDbContext.SaveChanges();
                    ViewBag.Status = 1;
                }
                catch
                {
                    ViewBag.Status = 0;
                }
            }
           
            catch(Exception ex)
            {
                Console.WriteLine("Error -" + ex);

                
            }


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


        //For sending email

        //public IActionResult EmailIndex()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult EmailIndex(EmailModel model)
        //{
        //    using (MailMessage mm = new MailMessage(model.Email, model.To))
        //    {
        //        mm.Subject = model.Subject;
        //        mm.Body = model.Body;
        //        //if (model.Attachment.Length > 0)
        //        //{
        //        //    string fileName = Path.GetFileName(model.Attachment.FileName);
        //        //    mm.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), fileName));
        //        //}
        //        mm.IsBodyHtml = false;
        //        using (SmtpClient smtp = new SmtpClient())
        //        {
        //            smtp.Host = "smtp.gmail.com";
        //            smtp.EnableSsl = true;
        //            NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
        //            smtp.UseDefaultCredentials = true;
        //            smtp.Credentials = NetworkCred;
        //            smtp.Port = 587;
        //            smtp.Send(mm);
        //            ViewBag.Message = "Email sent.";
        //        }
        //    }

        //    return View();
        //}















        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}