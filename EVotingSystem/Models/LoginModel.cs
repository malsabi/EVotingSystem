using Google.Cloud.Firestore;

namespace EVotingSystem.Models
{
    [FirestoreData]
    public class LoginModel
    {
        #region "Properties"
        [FirestoreProperty]
        public string StudentId { get; set; }
        [FirestoreProperty]
        public string Password { get; set; }
        [FirestoreProperty]
        public bool StaySignedIn { get; set; }
        #endregion
    }
}