using EVotingSystem.Helpers;
using Google.Cloud.Firestore;

namespace EVotingSystem.Models.Candidate
{
    [FirestoreData]
    public class CandidateModel
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Gender { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Speach { get; set; }

        [FirestoreProperty]
        public string Image { get; set; }

        [FirestoreProperty]
        public string LastVoteReceived { get; set; }

        public void EncryptProperties()
        {
            this.Name   = CandidateHelper.EncryptField(this.Name);
            this.Gender = CandidateHelper.EncryptField(this.Gender);
            this.Id     = CandidateHelper.EncryptField(this.Id);
            this.Speach = CandidateHelper.EncryptField(this.Speach);
            this.Image  = CandidateHelper.EncryptField(this.Image);
        }
        public void DecryptProperties()
        {
            this.Name   = CandidateHelper.DecryptField(this.Name);
            this.Gender = CandidateHelper.DecryptField(this.Gender);
            this.Id     = CandidateHelper.DecryptField(this.Id);
            this.Speach = CandidateHelper.DecryptField(this.Speach);
            this.Image  = CandidateHelper.DecryptField(this.Image);
        }

        public override string ToString()
        {
            return string.Concat("[", Name, ", ", Gender, ", ", Id, ", ", Speach, " ", Image, "]");
        }
    }
}