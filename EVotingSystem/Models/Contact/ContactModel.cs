using EVotingSystem.Validation;
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Models.Contact
{
    [FirestoreData]
    public class ContactModel
    {
        [FirestoreProperty]
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
        [Required(ErrorMessage = "Please insert your email")]
        [MaxLength(19, ErrorMessage = "Email cannot exceed more than 19 characters")]
        [EmailValidation(ErrorMessage = "Invalid email, please insert a valid HCT email")]
        public string EmailAddress { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your phone number")]
        [MaxLength(10, ErrorMessage = "Phone number cannot exceed more than 10 digits")]
        public string PhoneNumber { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your comment")]
        public string Comment { get; set; }
    }
}