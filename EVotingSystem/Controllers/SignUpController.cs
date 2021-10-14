using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class SignUpController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        #endregion

        public SignUpController()
        {
            FireStore = new FireStoreManager();
            Identity = new IdentityHandler(this);
        }

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            //Check if the cookie exists there, if so and (signed in), change the view.
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
        public IActionResult Successful()
        {
            //Check if the cookie exists there, if so and (signed in), change the view.
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
        public IActionResult Index(SignUpModel model)
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (FireStore.IsStudentRegistered(model.StudentId).Result)
                    {
                        return Json(new { State = "Error" });
                    }
                    else
                    {
                        string ConfirmCode = DeviceHelper.GetVerificationCode(Config.ConfirmCodeLength);
                        VerificationHelper.SendCode(model.FirstName, model.LastName, model.Email, ConfirmCode);
                        Identity.SetConfirmationCode(ConfirmCode);
                        return Json(new { State = "Valid", ConfirmCode });
                    }
                }
                else
                {
                    return Json(new { State = "Invalid" });
                }
            }
        }
        [HttpPost]
        public IActionResult Check(SignUpModel model)
        {
            if (Identity.IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string ConfirmCode = Identity.GetConfirmationCode();
                if (ConfirmCode != null)
                {
                    if (ConfirmCode.Equals(model.Code))
                    {
                        StudentModel Student = new StudentModel()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            NationalId = model.NationalId,
                            StudentId = model.StudentId,
                            Email = model.Email,
                            Password = model.Password,
                            Phone = model.Phone,
                            Status = "Offline",
                            StaySignedIn = "false",
                            ExpiryDate = System.DateTime.UtcNow,
                            TotalVotesApplied = "0",
                            SentVotes = null
                        };
                        FireStore.RegisterStudent(Student);
                        Identity.DeleteConfirmationCode();

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
        }
        #endregion
    }
}