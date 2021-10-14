using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace EVotingSystem.Models
{
    [FirestoreData]
    public class CandidateModel
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Gender { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Speach { get; set; }

        [FirestoreProperty]
        public string Image { get; set; }

        [FirestoreProperty]
        public string TotalVotes { get; set; }

        [FirestoreProperty]
        public DateTime LastVoteReceived { get; set; }
    }
}