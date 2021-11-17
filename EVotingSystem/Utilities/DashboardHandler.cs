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

        #region "Structs"
        public struct PercentageVotes
        {
            public int TotalMaleVotes;
            public int TotalFemaleVotes;
        }
        #endregion

        public DashboardHandler()
        {
            FireStore = new FireStoreManager();
        }

        #region "Public Methods"
        public DashboardModel GetDashboardInformation()
        {
            DashboardModel Dashboard = new DashboardModel
            {
                DueDate = FireStore.GetResult().DueDate
            };
            List<StudentModel> Students = FireStore.GetAllStudents(true).Result;
            List<CandidateModel> Candidates = FireStore.GetAllCandidates(true).Result;
            List<CandidateVoteModel> Votes = FireStore.GetAllCandidateVotes(true).Result;


            Dashboard.Students = Students.OrderBy(s => s.FirstName).ToList();
            Dashboard.Candidates = Candidates.OrderBy(c => c.Name).ToList();

            int StudentsCount = Students.Count > 0 ? Students.Count : 1;
            int CandidatesCount = Candidates.Count > 0 ? Candidates.Count : 1;
            int VotesCount = Votes.Count > 0 ? Votes.Count : 1;

            Dashboard.TotalRegisteredStudents = Students.Count;
            Dashboard.TotalMaleStudents = (int)(Students.Where(s => s.Gender.Equals(Config.Male)).ToList().Count() / (double)StudentsCount * 100);
            Dashboard.TotalFemaleStudents = (int)(Students.Where(s => s.Gender.Equals(Config.Female)).ToList().Count() / (double)StudentsCount * 100);

            Dashboard.TotalActiveCandidates = Candidates.Count;
            Dashboard.TotalActiveMaleCandidates = (int)(Candidates.Where(s => s.Gender.Equals(Config.Male)).ToList().Count() / (double)CandidatesCount * 100);
            Dashboard.TotalActiveFemaleCandidates = (int)(Candidates.Where(s => s.Gender.Equals(Config.Female)).ToList().Count() / (double)CandidatesCount * 100);

            PercentageVotes percentageVotes = GetPercentageVotes(Votes, VotesCount);
            Dashboard.TotalAttemptedVotes = Votes.Sum(c => int.Parse(c.TotalVotes));
            Dashboard.TotalAttemptedMaleVotes = percentageVotes.TotalMaleVotes;
            Dashboard.TotalAttemptedFemaleVotes = percentageVotes.TotalFemaleVotes;

            return Dashboard;
        }
        #endregion

        #region "Private Methods"
        public PercentageVotes GetPercentageVotes(List<CandidateVoteModel> Votes, int VotesCount)
        {
            int MaleSum = 0;
            int FemaleSum = 0;

            PercentageVotes percentageVotes = new PercentageVotes();

            foreach (CandidateVoteModel CV in Votes)
            {
                if (CV.StudentVoteCollection != null)
                {
                    foreach (StudentVoteModel SV in CV.StudentVoteCollection)
                    {
                        SV.DecryptProperties();
                        if (SV.Gender.Equals(Config.Male))
                        {
                            MaleSum += 1;
                        }
                        else
                        {
                            FemaleSum += 1;
                        }
                    }
                }
            }
            percentageVotes.TotalMaleVotes = (int)(MaleSum / (double)VotesCount * 100);
            percentageVotes.TotalFemaleVotes = (int)(FemaleSum / (double)VotesCount * 100);

            return percentageVotes;
        }
        #endregion
    }
}