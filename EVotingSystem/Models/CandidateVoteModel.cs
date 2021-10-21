using EVotingSystem.Helpers;
using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace EVotingSystem.Models
{
    [FirestoreData]
    public class CandidateVoteModel
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string TotalVotes { get; set; }

        [FirestoreProperty]
        public List<StudentVoteModel> StudentVoteCollection { get; set; }

        public void EncryptProperties()
        {
            this.Id         = CandidateHelper.EncryptField(this.Id);
            this.TotalVotes = CandidateHelper.EncryptField(this.TotalVotes);
        }

        public void DecryptProperties()
        {
            this.Id         = CandidateHelper.DecryptField(this.Id);
            this.TotalVotes = CandidateHelper.DecryptField(this.TotalVotes);
        }
    }
}