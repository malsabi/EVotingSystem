using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Models.Admin;
using EVotingSystem.Models.Identity;
using EVotingSystem.Models.Student;
using EVotingSystem.Utilities;
using EVotingSystem.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            if (Identity.IsStudentLoggedIn() || Identity.IsAdminLoggedIn())
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
            if (Identity.IsStudentLoggedIn() || Identity.IsAdminLoggedIn())
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
            Login.EncryptProperties();

            if (Identity.IsStudentLoggedIn() || Identity.IsAdminLoggedIn())
            {
                return RedirectToPage("Index", "Home");
            }
            else
            {
                switch (Login.AccountType)
                {
                    case "Student":
                        {
                            if (ModelState["StudentId"].Errors.Count() > 0)
                            {
                                return Json(new { State = "Invalid" });
                            }
                            else
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
                                        Student.DecryptProperties();
                                        string ConfirmationCode = DeviceHelper.GetVerificationCode(Config.ConfirmCodeLength);
                                        VerificationHelper.SendCode(Student.FirstName, Student.LastName, Student.Email, ConfirmationCode);
                                        Identity.SetConfirmationCode(ConfirmationCode);
                                        ServiceLogger.Log(LogType.Student, LogLevel.Info, Config.StudentInfoTitle, string.Format("{0}: {1}", Student.StudentId, Config.StudentInfoLoginMessage));
                                        return Json(new { State = "Valid", ConfirmationCode });
                                    }
                                    else
                                    {
                                        return Json(new { State = "ErrorPassword" });
                                    }
                                }
                            }
                        }
                    case "Administrator":
                        {
                            if (ModelState["Email"].Errors.Count() > 0)
                            {
                                return Json(new { State = "Invalid" });
                            }
                            else
                            {
                                if (FireStore.IsAdminRegistered(Login.Email).Result == false)
                                {
                                    return Json(new { State = "Error" });
                                }
                                else if (FireStore.IsAdminOnline(Login.Email).Result == true)
                                {
                                    return Json(new { State = "ErrorActive" });
                                }
                                else
                                {
                                    AdminModel Admin = FireStore.GetAdmin(Login.Email).Result;
                                    if (Admin.Password.Equals(Login.Password))
                                    {
                                        FireStore.LoginAdmin(Login);
                                        Identity.LoginAdmin(Login);
                                        ServiceLogger.Log(LogType.Admin, LogLevel.Info, Config.AdminInfoTitle, Config.AdminInfoSuccessLoginMessage);
                                        return Json(new { State = "Success", Login });
                                    }
                                    else
                                    {
                                        return Json(new { State = "ErrorPassword" });
                                    }
                                }
                            }
                        }
                    default:
                        return Json(new { State = "Invalid" });
                }
            }
        }

        [HttpPost]
        public IActionResult Check(LoginModel Login)
        {
            Login.EncryptProperties();

            if (Identity.IsStudentLoggedIn() || Identity.IsAdminLoggedIn())
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
                        Identity.LoginStudent(Login);

                        Login.DecryptProperties();
                        ServiceLogger.Log(LogType.Student, LogLevel.Info, Config.StudentInfoTitle, string.Format("{0}: {1}", Login.StudentId, Config.StudentInfoSuccessLoginMessage));
                        Login.EncryptProperties();
                        return Json(new { State = "Success", Login });
                    }
                    else
                    {
                        ServiceLogger.Log(LogType.Student, LogLevel.Info, Config.StudentInfoTitle, string.Format("{0}: {1}", Login.StudentId, Config.StudentInfoFailedLoginMessage));
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
            if (Identity.IsStudentLoggedIn() || Identity.IsAdminLoggedIn())
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