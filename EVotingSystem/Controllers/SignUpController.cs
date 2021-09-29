using EVotingSystem.DataBase;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace EVotingSystem.Controllers
{
    public class SignUpController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FS = new FireStoreManager();
        private static string Code = null;
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
                    Code = DeviceHelper.GetVerificationCode(25);
                    SendCodeViaEmail(model, Code);
                    return Json(new { State = "Valid", Code });
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
            if (Code.Equals(model.Code))
            {
                FS.AddRegisteredUser(model);
                return Json(new { State = "Success", model });
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
        #endregion
    }
}