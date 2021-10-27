using EVotingSystem.Helpers;
using Google.Cloud.Firestore;

namespace EVotingSystem.Models.Admin
{
    [FirestoreData]
    public class AdminModel
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Gender { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public string StaySignedIn { get; set; }

        [FirestoreProperty]
        public string Status { get; set; }

        public void EncryptProperties()
        {
            this.Name     = AdminHelper.EncryptField(this.Name);
            this.Email    = AdminHelper.EncryptField(this.Email);
            this.Password = AdminHelper.EncryptField(this.Password);
        }

        public void DecryptProperties()
        {
            this.Name     = AdminHelper.DecryptField(this.Name);
            this.Email    = AdminHelper.DecryptField(this.Email);
            this.Password = AdminHelper.DecryptField(this.Password);
        }
    }
}