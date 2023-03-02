using CIPlatform.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;


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
            CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();
            var status = _ciplatformDbContext.Users.Where(u => u.Email == _loginModel.LoginId && u.Password == _loginModel.Password).FirstOrDefault();

            if (status == null)
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

                var generated_token = Guid.NewGuid().ToString();  //for token generation


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






        //[HttpGet]
        //public IActionResult ResetPass()
        //{
        //    return View();
        //}



        //[HttpPost]
        //public IActionResult ResetPass(LoginModel _loginModel, PasswordReset _passwordReset)
        //{
        //    CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();

        //    var generated_pass = Request.Form["pass1"];
        //    var current_url = HttpContext.Request.GetDisplayUrl();

        //    // for spliting the url

        //    string path = new Uri(current_url).LocalPath.Substring(21);
        //    Console.WriteLine(path);
        //    Console.WriteLine(generated_pass);


        //    var generated_token = _ciplatformDbContext.PasswordResets.FirstOrDefault(x => (x.Token == path));
        //    if (generated_token != null)
        //    {

        //        var emailtemp = generated_token.Email;
        //        var check_email = _ciplatformDbContext.Users.FirstOrDefault(u => (u.Email == emailtemp.ToLower()));

        //        if (check_email != null)
        //        {

        //            check_email.Password = generated_pass;
        //            _ciplatformDbContext.Users.Update(check_email);
        //            _ciplatformDbContext.SaveChanges();

        //            return RedirectToAction("Index", "Home");

        //        }


        //        else
        //        {
        //            return RedirectToAction("ResetPass", "Home");

        //        }

        //    }



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