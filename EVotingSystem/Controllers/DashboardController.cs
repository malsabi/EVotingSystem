using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Logger;
using EVotingSystem.Models.Candidate;
using EVotingSystem.Models.Dashboard;
using EVotingSystem.Models.Student;
using EVotingSystem.Utilities;
using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EVotingSystem.Controllers
{
    public class DashboardController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        private readonly DashboardHandler Dashboard;
        #endregion

        public DashboardController()
        {
            FireStore = new FireStoreManager();
            Identity = new IdentityHandler(this);
            Dashboard = new DashboardHandler();
        }

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            DashboardModel DashboardModel = Dashboard.GetDashboardInformation();
            List<PieSeriesData> StudentPieData = new List<PieSeriesData>
            {
                new PieSeriesData { Name = "Male", Y = DashboardModel.TotalMaleStudents },
                new PieSeriesData { Name = "Female", Y = DashboardModel.TotalFemaleStudents }
            };

            List<PieSeriesData> CandidatePieData = new List<PieSeriesData>
            {
                new PieSeriesData { Name = "Male", Y = DashboardModel.TotalActiveMaleCandidates },
                new PieSeriesData { Name = "Female", Y = DashboardModel.TotalActiveFemaleCandidates }
            };

            List<PieSeriesData> CandidateVotesPieData = new List<PieSeriesData>
            {
                new PieSeriesData { Name = "Male", Y = DashboardModel.TotalAttemptedMaleVotes },
                new PieSeriesData { Name = "Female", Y = DashboardModel.TotalAttemptedFemaleVotes }
            };


            ViewData["StudentPieData"] = StudentPieData;
            ViewData["CandidatePieData"] = CandidatePieData;
            ViewData["CandidateVotesPieData"] = CandidateVotesPieData;

            //Only admin can access the dashboard.
            if (Identity.IsAdminLoggedIn() && Identity.IsStudentLoggedOut())
            {
                return View(DashboardModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult DeleteStudent(string Id)
        {
            if (Identity.IsAdminLoggedIn() && Identity.IsStudentLoggedOut())
            {
                StudentModel Student = FireStore.GetStudent(StudentHelper.EncryptField(Id)).Result;
                Student.DecryptProperties();
                FireStore.RemoveStudent(StudentHelper.EncryptField(Id));
                ServiceLogger.Log(LogType.Admin, LogLevel.Get, Config.AdminGetTitle, string.Format("Student Deleted: {0}", Student.ToString()));
                return Json(new { State = "Success" });
            }
            else
            {
                return Json(new { State = "Error" });
            }
        }

        [HttpPost]
        public IActionResult DeleteCandidate(string Id)
        {
            if (Identity.IsAdminLoggedIn() && Identity.IsStudentLoggedOut())
            {
                CandidateModel Candidate = FireStore.GetCandidate(CandidateHelper.EncryptField(Id)).Result;
                Candidate.DecryptProperties();
                FireStore.RemoveCandidate(CandidateHelper.EncryptField(Id));
                ServiceLogger.Log(LogType.Admin, LogLevel.Get, Config.AdminGetTitle, string.Format("Candidate Deleted: {0}", Candidate.ToString()));
                return Json(new { State = "Success" });
            }
            else
            {
                return Json(new { State = "Error" });
            }
        }

        [HttpPost]
        public IActionResult AddCandidate(string Candidate)
        {
            //Information Phase
            string[] CandidateDate = Candidate.Split("|");
            
            //Validation Phase
            if (CandidateDate.Length != 5)
            {
                return Json(new { State = "Error" });
            }
            else
            {
                string Name = CandidateDate[0], Gender = CandidateDate[1], Id = CandidateDate[2], Speach = CandidateDate[3], ImageBase64 = CandidateDate[4];
                if (Name.Any(c => char.IsDigit(c)))
                {
                    return Json(new { State = "Error", Reason = "Name cannot contain digits" });
                }
                else if (!Gender.Equals(Config.Male) && !Gender.Equals(Config.Female))
                {
                    return Json(new { State = "Error", Reason = "Invalid Gender" });
                }
                else if (Id.Length != 4 || Id.Any(c => (char.IsDigit(c) == false)))
                {
                    return Json(new { State = "Error", Reason = "Id is only 4 digits"});
                }
                else
                {
                    //Checking Phase
                    CandidateModel candidateModel = new CandidateModel()
                    {
                        Name = Name,
                        Gender = Gender,
                        Id = Id,
                        Speach = Speach,
                        Image = ImageBase64
                    };
                    candidateModel.EncryptProperties();

                    if (FireStore.IsCandidateAdded(candidateModel.Id).Result)
                    {
                        return Json(new { State = "Error", Reason = "Candidate already added" });
                    }
                    else
                    {
                        //Insertion Phase
                        FireStore.AddCandidate(candidateModel);
                        return Json(new { State = "Success", Name, Gender, Id });
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult AddDueDate(string DueDate)
        {
            if (DateHelper.IsDateValid(DueDate))
            {
                if (DateHelper.IsDateBeforeOrEqual(DueDate))
                {
                    return Json(new { State = "Invalid" });
                }
                else
                {
                    ResultModel Result = new ResultModel
                    {
                        DueDate = DueDate,
                        WinnerCandidate = null
                    };
                    FireStore.AddDueDate(Result);
                    return Json(new { State = "Success", DueDate });
                }
           
            }
            else
            {
                return Json(new { State = "Error" });
            }
        }

        [HttpPost]
        public IActionResult DeleteDueDate()
        {
            if (FireStore.IsDueDateAdded())
            {
                FireStore.DeleteDueDate();
                return Json(new { State = "Success" });
            }
            else
            {
                return Json(new { State = "Error" });
            }
        }
        #endregion
    }
}