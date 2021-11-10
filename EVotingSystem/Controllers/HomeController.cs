using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Logger;
using EVotingSystem.Models;
using EVotingSystem.Models.Student;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EVotingSystem.Controllers
{
    public class HomeController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        #endregion

        public HomeController()
        {
            FireStore = new FireStoreManager();
            Identity = new IdentityHandler(this);
        }

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            if (Identity.IsStudentLoggedIn())
            {
                return View(Identity.StudentSession());
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Identity.IsStudentLoggedIn())
            {
                StudentModel Student = Identity.StudentSession();
                //Change status in the database
                FireStore.LogoutStudent(Student);
                //Remove the user session cookie.
                Identity.LogoutStudent();
                ServiceLogger.Log(LogType.Student, LogLevel.Get, Config.StudentGetTitle, string.Format("{0}: {1}", Student.StudentId, Config.StudentGetLogoutMessage));
            }
            else if (Identity.IsAdminLoggedIn())
            {
                //Change status in the database
                FireStore.LogoutAdmin(Identity.AdminSession());
                //Remove the user session cookie.
                Identity.LogoutAdmin();
                ServiceLogger.Log(LogType.Admin, LogLevel.Get, Config.AdminGetTitle, Config.AdminGetLogoutMessage);
            }
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}