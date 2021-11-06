using EVotingSystem.Helpers;
using Google.Cloud.Firestore;

namespace EVotingSystem.Models.Student
{
    [FirestoreData]
    public class StudentVoteModel
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Gender { get; set; }

        public void EncryptProperties()
        {
            this.Id     = StudentHelper.EncryptField(this.Id);
            this.Gender = StudentHelper.EncryptField(this.Gender);
        }

        public void DecryptProperties()
        {
            this.Id     = StudentHelper.DecryptField(this.Id);
            this.Gender = StudentHelper.DecryptField(this.Gender);
        }
    }
}