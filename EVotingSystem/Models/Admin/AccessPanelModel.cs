using EVotingSystem.Helpers;
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Models.Admin
{
    [FirestoreData]
    public class AccessPanelModel
    {
        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your email")]
        [MaxLength(30, ErrorMessage = "Email cannot exceed more than 30 characters")]
        public string Email { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Please insert your password")]
        [MaxLength(30, ErrorMessage = "Password cannot exceed more than 30 characters")]
        public string Password { get; set; }

        [FirestoreProperty]
        public string StaySignedIn { get; set; }


        public void EncryptProperties()
        {
            this.Email    = AdminHelper.EncryptField(this.Email);
            this.Password = AdminHelper.EncryptField(this.Password);
        }
        public void DecryptProperties()
        {
            this.Email = AdminHelper.DecryptField(this.Email);
            this.Password = AdminHelper.DecryptField(this.Password);
        }
    }
}