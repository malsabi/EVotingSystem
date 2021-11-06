using EVotingSystem.Cryptography;
using EVotingSystem.Helpers;
using EVotingSystem.Validation;
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Models.Identity
{
    [FirestoreData]
    public class SignUpModel
    {
        [FirestoreProperty] //Serialize
        [Required(ErrorMessage = "Please insert your first name")]
        [MaxLength(30, ErrorMessage = "Cannot exceed more than 30 letters")]
        [NameValidation(ErrorMessage = "Invalid First Name, Please use letters only")]
        public string FirstName { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your last name")]
        [MaxLength(30, ErrorMessage = "Cannot exceed more than 30 letters")]
        [NameValidation(ErrorMessage = "Invalid Last Name, please use letters only")]
        public string LastName { get; set; }


        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your National Id")]
        [MaxLength(30, ErrorMessage = "National Id cannot exceed more than 18 characters")]
        [NationalIdValidation( ErrorMessage = "Invalid National Id, please insert a valid UAE Id")]
        public string NationalId { get; set; }


        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your Student Id")]
        [MaxLength(9, ErrorMessage = "Student Id cannot exceed more than 9 characters")]
        [StudentIdValidation(ErrorMessage = "Invalid Student Id, please insert a valid HCT Id")]
        public string StudentId { get; set; }


        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your email")]
        [MaxLength(19, ErrorMessage = "Email cannot exceed more than 19 characters")]
        [EmailValidation(ErrorMessage = "Invalid email, please insert a valid HCT email")]
        public string Email { get; set; }


        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your password")]
        [MaxLength(30, ErrorMessage = "Password cannot exceed more than 30 characters")]
        public string Password { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your phone number")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed more than 10 digits")]
        public string Phone { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your gender")]
        [MaxLength(6, ErrorMessage = "gender cannot exceed more than 6 letters")]
        [GenderValidation(ErrorMessage = "Invalid gender, please insert a valid Gender")]
        public string Gender { get; set; }

        public string Code { get; set; }

        public void EncryptProperties()
        {
            this.StudentId = FirestoreEncoder.EncodeForFirebaseKey(CandidateHelper.EncryptField(this.StudentId));
        }

        public void DecryptProperties()
        {
            this.StudentId = FirestoreEncoder.DecodeFromFirebaseKey(CandidateHelper.DecryptField(this.StudentId));
        }
    }
}