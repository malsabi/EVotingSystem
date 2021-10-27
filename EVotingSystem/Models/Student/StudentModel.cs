using EVotingSystem.Helpers;
using Google.Cloud.Firestore;
using System;

namespace EVotingSystem.Models.Student
{
    [FirestoreData]
    public class StudentModel
    {
        [FirestoreProperty]
        public string FirstName { get; set; }

        [FirestoreProperty]
        public string LastName { get; set; }

        [FirestoreProperty]
        public string NationalId { get; set; }

        [FirestoreProperty]
        public string StudentId { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public string Phone { get; set; }

        [FirestoreProperty]
        public string Gender { get; set; }

        [FirestoreProperty]
        public string Status { get; set; }

        [FirestoreProperty]
        public string StaySignedIn { get; set; }

        [FirestoreProperty]
        public DateTime ExpiryDate { get; set; }

        [FirestoreProperty]
        public string TotalVotesApplied { get; set; }


        public void EncryptProperties()
        {
            this.FirstName  =  StudentHelper.EncryptField(this.FirstName);
            this.LastName   =  StudentHelper.EncryptField(this.LastName);
            this.NationalId =  StudentHelper.EncryptField(this.NationalId);
            this.StudentId  =  StudentHelper.EncryptField(this.StudentId);
            this.Email      =  StudentHelper.EncryptField(this.Email);
            this.Password   =  StudentHelper.EncryptField(this.Password);
            this.Phone      =  StudentHelper.EncryptField(this.Phone);
        }

        public void DecryptProperties()
        {
            this.FirstName  =  StudentHelper.DecryptField(this.FirstName);
            this.LastName   =  StudentHelper.DecryptField(this.LastName);
            this.NationalId =  StudentHelper.DecryptField(this.NationalId);
            this.StudentId  =  StudentHelper.DecryptField(this.StudentId);
            this.Email      =  StudentHelper.DecryptField(this.Email);
            this.Password   =  StudentHelper.DecryptField(this.Password);
            this.Phone      =  StudentHelper.DecryptField(this.Phone);
        }
    }
}