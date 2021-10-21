using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class ProfileController : Controller
    {
        #region "Fields"
        private readonly IdentityHandler Identity;
        #endregion

        public ProfileController()
        {
            Identity = new IdentityHandler(this);
        }

        #region "GET"
        public IActionResult Index()
        {
            if (Identity.IsStudentLoggedIn())
            {
                return View(Identity.StudentSession());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #endregion
    }
}