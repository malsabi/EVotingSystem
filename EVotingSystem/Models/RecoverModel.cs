using Google.Cloud.Firestore;

namespace EVotingSystem.Models
{
    [FirestoreData]
    public class RecoverModel
    {
        #region "Properties"
        [FirestoreProperty]
        public string StudentID { get; set; }
        #endregion
    }
}