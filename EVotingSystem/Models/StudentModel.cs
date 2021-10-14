using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace EVotingSystem.Models
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
        public string Status { get; set; }

        [FirestoreProperty]
        public string StaySignedIn { get; set; }

        [FirestoreProperty]
        public DateTime ExpiryDate { get; set; }

        [FirestoreProperty]
        public string TotalVotesApplied { get; set; }

        [FirestoreProperty]
        public List<CandidateModel> SentVotes { get; set; }
    }
}