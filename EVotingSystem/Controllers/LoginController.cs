using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class LoginController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        #endregion


        public LoginController()
        {
            FireStore = new FireStoreManager();
            Identity = new IdentityHandler(this);
        }



        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Recover()
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region "POST"
        [HttpPost] 
        public IActionResult Index(LoginModel Login)
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToPage("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (FireStore.IsStudentRegistered(Login.StudentId).Result == false)
                    {
                        return Json(new { State = "Error" });
                    }
                    else if (FireStore.IsStudentOnline(Login.StudentId).Result == true)
                    {
                        return Json(new { State = "ErrorActive" });
                    }
                    else
                    {
                        StudentModel Student = FireStore.GetStudent(Login.StudentId).Result;

                        if (Student.Password.Equals(Login.Password))
                        {
                            string ConfirmationCode = DeviceHelper.GetVerificationCode(Config.ConfirmCodeLength);

                            VerificationHelper.SendCode(Student.FirstName, Student.LastName, Student.Email, ConfirmationCode);

                            Identity.SetConfirmationCode(ConfirmationCode);

                            return Json(new { State = "Valid", ConfirmationCode });
                        }
                        else
                        {
                            return Json(new { State = "ErrorPassword" });
                        }
                    }
                }
                else
                {
                    return Json(new { State = "Invalid" });
                }
            }
        }

        [HttpPost]
        public IActionResult Check(LoginModel Login)
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToPage("Index", "Home");
            }
            else
            {
                string ConfirmCode = Identity.GetConfirmationCode();
                if (ConfirmCode != null)
                {
                    if (ConfirmCode.Equals(Login.Code))
                    {
                        //Change status in firebase
                        FireStore.LoginStudent(Login);

                        //Remove confirmation code cookie
                        Identity.DeleteConfirmationCode();

                        //Add the user session cookie.
                        Identity.LogIn(Login);
                        
                        return Json(new { State = "Success", Login });
                    }
                    else
                    {
                        return Json(new { State = "Failed", Login });
                    }
                }
                else
                {
                    return Json(new { State = "Failed", Login });
                }
            }
        }

        [HttpPost]
        public IActionResult Recover(RecoverModel model)
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToPage("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
        #endregion
    }
}