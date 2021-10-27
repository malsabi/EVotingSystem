using EVotingSystem.DataBase;
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
            List<PieSeriesData> StudentPieData = new List<PieSeriesData>();
            StudentPieData.Add(new PieSeriesData { Name = "Male", Y = DashboardModel.TotalMaleStudents });
            StudentPieData.Add(new PieSeriesData { Name = "Female", Y = DashboardModel.TotalFemaleStudents });

            List<PieSeriesData> CandidatePieData = new List<PieSeriesData>();
            CandidatePieData.Add(new PieSeriesData { Name = "Male", Y = DashboardModel.TotalActiveMaleCandidates });
            CandidatePieData.Add(new PieSeriesData { Name = "Female", Y = DashboardModel.TotalActiveFemaleCandidates });

            List<PieSeriesData> CandidateVotesPieData = new List<PieSeriesData>();
            CandidateVotesPieData.Add(new PieSeriesData { Name = "Male", Y = DashboardModel.TotalAttemptedMaleVotes });
            CandidateVotesPieData.Add(new PieSeriesData { Name = "Female", Y = DashboardModel.TotalAttemptedFemaleVotes });


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
        #endregion
    }
}