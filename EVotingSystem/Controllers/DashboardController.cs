using EVotingSystem.Models.Dashboard;
using EVotingSystem.Utilities;
using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EVotingSystem.Controllers
{
    public class DashboardController : Controller
    {
        #region "Fields"
        private readonly IdentityHandler Identity;
        private readonly DashboardHandler Dashboard;
        #endregion

        public DashboardController()
        {
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
                return Json(new { Status = Id });
            }
            else
            {
                return Json(new { Status = "Error" });
            }
        }

        [HttpPost]
        public IActionResult DeleteCandidate(string Id)
        {
            if (Identity.IsAdminLoggedIn() && Identity.IsStudentLoggedOut())
            {
                return Json(new { Status = Id });
            }
            else
            {
                return Json(new { Status = "Error" });
            }
        }
        #endregion
    }
}