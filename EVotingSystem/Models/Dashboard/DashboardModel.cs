using EVotingSystem.Models.Candidate;
using EVotingSystem.Models.Student;
using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace EVotingSystem.Models.Dashboard
{
    [FirestoreData]
    public class DashboardModel
    {
        [FirestoreProperty]
        public List<StudentModel> Students { get; set; }

        [FirestoreProperty]
        public List<CandidateModel> Candidates { get; set; }

        [FirestoreProperty]
        public int TotalRegisteredStudents { get; set; }

        [FirestoreProperty]
        public int TotalMaleStudents { get; set; }

        [FirestoreProperty]
        public int TotalFemaleStudents { get; set; }

        [FirestoreProperty]
        public int TotalActiveCandidates { get; set; }

        [FirestoreProperty]
        public int TotalActiveFemaleCandidates { get; set; }

        [FirestoreProperty]
        public int TotalActiveMaleCandidates { get; set; }

        [FirestoreProperty]
        public int TotalAttemptedVotes { get; set; }

        [FirestoreProperty]
        public int TotalAttemptedFemaleVotes { get; set; }

        [FirestoreProperty]
        public int TotalAttemptedMaleVotes { get; set; }
    }
}