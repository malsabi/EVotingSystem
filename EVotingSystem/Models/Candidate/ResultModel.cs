using Google.Cloud.Firestore;

namespace EVotingSystem.Models.Candidate
{
    [FirestoreData]
    public class ResultModel
    {
        [FirestoreProperty]
        public string DueDate { get; set; }

        [FirestoreProperty]
        public CandidateModel WinnerCandidate { get; set; }

        [FirestoreProperty]
        public string TotalVotes { get; set; }
    }
}