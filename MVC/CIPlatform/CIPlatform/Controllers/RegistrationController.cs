using CIPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    public class RegistrationController : Controller
    {
        //[HttpGet]
        //public IActionResult Registration()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Registration(RegistrationModel _registrationModel)
        //{
        //    CiplatformDbContext _ciplatformDbContext = new CiplatformDbContext();

        //    try
        //    {
        //        var userData = new User()
        //        {
        //            FirstName = _registrationModel.FirstName,
        //            LastName = _registrationModel.LastName,
        //            PhoneNumber = _registrationModel.PhoneNumber,
        //            Email = _registrationModel.Email,
        //            Password = _registrationModel.Password

        //        };
        //        _ciplatformDbContext.Users.Add(userData);
        //        _ciplatformDbContext.SaveChanges();
        //        ViewBag.Status = 1;
        //    }
        //    catch
        //    {
        //        ViewBag.Status = 0;
        //    }


        //    return View();
        //}
    }
}
