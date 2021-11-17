using EVotingSystem.Helpers;
using EVotingSystem.Models.Contact;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EVotingSystem.Controllers
{
    public class ContactController : Controller
    {
        #region "Fields"
        #endregion

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(ContactModel Contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContactHelper.SendUserContact(Contact);
                    return Json(new { State = "Success" });
                }
                catch (Exception)
                {
                    return Json(new { State = "Error" });
                }
            }
            else
            {
                return Json(new { State = "Error" });
            }
        }
        #endregion
    }
}
