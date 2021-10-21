using EVotingSystem.DataBase;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class AccessPanelController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        #endregion

        public AccessPanelController()
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
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(AccessPanelModel AccessPanel)
        {
            AccessPanel.EncryptProperties();

            if (Identity.IsAdminLoggedIn())
            {
                return RedirectToPage("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (FireStore.IsAdminRegistered(AccessPanel.Email).Result == false)
                    {
                        return Json(new { State = "Error" });
                    }
                    else if (FireStore.IsAdminOnline(AccessPanel.Email).Result == true)
                    {
                        return Json(new { State = "ErrorActive" });
                    }
                    else
                    {
                        AdminModel Admin = FireStore.GetAdmin(AccessPanel.Email).Result;
                        if (Admin.Password.Equals(AccessPanel.Password))
                        {
                            //Change status in firebase
                            FireStore.LoginAdmin(AccessPanel);
                            //Add the user session cookie.
                            Identity.LoginAdmin(AccessPanel);

                            return Json(new { State = "Success", AccessPanel });
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
        #endregion
    }
}