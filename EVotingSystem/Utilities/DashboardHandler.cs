using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Models.Candidate;
using EVotingSystem.Models.Dashboard;
using EVotingSystem.Models.Student;
using System.Collections.Generic;
using System.Linq;

namespace EVotingSystem.Utilities
{
    public class DashboardHandler
    {

        #region "Fields"
        private readonly FireStoreManager FireStore;
        #endregion

        public DashboardHandler()
        {
            FireStore = new FireStoreManager();
        }

        #region "Public Methods"
        public DashboardModel GetDashboardInformation()
        {
            DashboardModel Dashboard = new DashboardModel();

            List<StudentModel> Students = FireStore.GetAllStudents(true).Result;
            List<CandidateModel> Candidates = FireStore.GetAllCandidates(true).Result;
            List<CandidateVoteModel> Votes = FireStore.GetAllCandidateVotes(true).Result;

            Dashboard.Students = Students.OrderBy(s => s.FirstName).ToList();
            Dashboard.Candidates = Candidates.OrderBy(c => c.Name).ToList();

            //FIX DIVISION BY ZERO
            Dashboard.TotalRegisteredStudents = Students.Count;
            Dashboard.TotalMaleStudents = (int)(Students.Where(s => s.Gender.Equals(Config.Male)).ToList().Count() / (double)Students.Count * 100);
            Dashboard.TotalFemaleStudents = (int)(Students.Where(s => s.Gender.Equals(Config.Female)).ToList().Count() / (double)Students.Count * 100);

            //FIX DIVISION BY ZERO
            Dashboard.TotalActiveCandidates = Candidates.Count;
            Dashboard.TotalActiveMaleCandidates = (int)(Candidates.Where(s => s.Gender.Equals(Config.Male)).ToList().Count() / (double)Candidates.Count * 100);
            Dashboard.TotalActiveFemaleCandidates = (int)(Candidates.Where(s => s.Gender.Equals(Config.Female)).ToList().Count() / (double)Candidates.Count * 100);

            Dashboard.TotalAttemptedVotes = Votes.Sum(c => int.Parse(c.TotalVotes));
            Dashboard.TotalAttemptedMaleVotes = 40;
            Dashboard.TotalAttemptedFemaleVotes = 60;

            return Dashboard;
        }
        #endregion
    }
}