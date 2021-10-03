using EVotingSystem.DataBase;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;

namespace EVotingSystem.Controllers
{
    public class SignUpController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FS = new FireStoreManager();
        #endregion

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Successful()
        {
            return View();
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                if (FS.IsUserRegistered(model).Result)
                {
                    return Json(new { State = "Error" });
                }
                else
                {
                    string ConfirmCode = DeviceHelper.GetVerificationCode(7);
                    SendCodeViaEmail(model, ConfirmCode);
                    SetConfirmationCode(ConfirmCode);
                    return Json(new { State = "Valid", ConfirmCode });
                }
            }
            else
            {
                return Json(new { State = "Invalid" });
            }
        }
        [HttpPost]
        public IActionResult Check(SignUpModel model)
        {
            string ConfirmCode = GetConfirmationCode();
            if (ConfirmCode != null)
            {
                if (ConfirmCode.Equals(model.Code))
                {
                    FS.AddRegisteredUser(model);
                    RemoveConfirmationCode();
                    return Json(new { State = "Success", model });
                }
                else
                {
                    return Json(new { State = "Failed", model });
                }
            }
            else
            {
                return Json(new { State = "Failed", model });
            }
        }
        #endregion

        #region "Private Methods"
        private void SendCodeViaEmail(SignUpModel model, string Code)
        {
            const string CompanyEmail = "evotingsystemuae@gmail.com";
            const string CompanyPassword = "Evoting@uae@1";

            string SirFullName = string.Format("Full Name: {0}", string.Concat(model.FirstName, " ", model.LastName));
            string ConfirmationCode = string.Format("Confirmation Code: {0}", Code);
            string Message = string.Concat(SirFullName, "\n", ConfirmationCode);

            MailMessage mailMessage = new MailMessage(CompanyEmail, model.Email, "Confirmation Code", Message);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(CompanyEmail, CompanyPassword)
            };
            client.Send(mailMessage);
        }

        private void SetConfirmationCode(string Code)
        {
            HttpContext.Session.SetString("Confirmation", Code);
        }
        private string GetConfirmationCode()
        {
            return HttpContext.Session.GetString("Confirmation");
        }
        private void RemoveConfirmationCode()
        {
            string ConfirmCode = GetConfirmationCode();
            if (ConfirmCode != null)
            {
                RemoveCookie("Confirmation");
            }
        }
        public void RemoveCookie(string CookieKey)
        {
            Response.Cookies.Append(CookieKey, "", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1)
            });
        }
        #endregion
    }
}