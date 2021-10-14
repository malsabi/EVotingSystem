using EVotingSystem.Validation;
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Models
{
    [FirestoreData]
    public class LoginModel
    {
        #region "Properties"

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your Student Id")]
        [MaxLength(9, ErrorMessage = "Student Id cannot exceed more than 9 characters")]
        [StudentIdValidation(ErrorMessage = "Invalid Student Id, please insert a valid HCT Id")]
        public string StudentId { get; set; }


        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your password")]
        [MaxLength(30, ErrorMessage = "Password cannot exceed more than 30 characters")]
        public string Password { get; set; }


        [FirestoreProperty]
        public string StaySignedIn { get; set; }

        public string Code { get; set; }
        #endregion
    }
}