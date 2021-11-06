using EVotingSystem.Helpers;
using EVotingSystem.Validation;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Models.Identity
{
    public class LoginModel
    {
        #region "Properties"
        [Required(ErrorMessage = "Please insert your Student Id")]
        [MaxLength(9, ErrorMessage = "Student Id cannot exceed more than 9 characters")]
        [StudentIdValidation(ErrorMessage = "Invalid Student Id, please insert a valid HCT Id")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Please insert your Email")]
        [MaxLength(30, ErrorMessage = "Email cannot exceed more than 30 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please insert your password")]
        [MaxLength(30, ErrorMessage = "Password cannot exceed more than 30 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please insert your Account Type")]
        [MaxLength(30, ErrorMessage = "Account Type cannot exceed more than 30 characters")]
        public string AccountType { get; set; }

        public string StaySignedIn { get; set; }

        public string Code { get; set; }
        #endregion

        public void EncryptProperties()
        {
            if (this.StudentId != null && this.StudentId.Length > 0)
            {
                this.StudentId = StudentHelper.EncryptField(this.StudentId);
            }
            else if (this.Email != null && this.Email.Length > 0)
            {
                this.Email = StudentHelper.EncryptField(this.Email);
            }
            this.Password = StudentHelper.EncryptField(this.Password);
        }

        public void DecryptProperties()
        {
            if (this.StudentId != null && this.StudentId.Length > 0)
            {
                this.StudentId = StudentHelper.DecryptField(this.StudentId);
            }
            else if (this.Email != null && this.Email.Length > 0)
            {
                this.Email = StudentHelper.DecryptField(this.Email);
            }
            this.Password = StudentHelper.DecryptField(this.Password);
        }
    }
}