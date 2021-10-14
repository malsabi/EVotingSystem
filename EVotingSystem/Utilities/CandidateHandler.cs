using EVotingSystem.DataBase;
using EVotingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EVotingSystem.Utilities
{
    public class CandidateHandler
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        #endregion

        public CandidateHandler()
        {
            FireStore = new FireStoreManager();
        }

        #region "Private Methods"
        private DateTime GetVoteTimeStamp()
        {
            DateTime TimeStamp = DateTime.Now;
            return DateTime.SpecifyKind(TimeStamp, DateTimeKind.Utc);
        }
        #endregion

        #region "Public Methods"
        public void ApplyVote(StudentModel Student, CandidateModel Candidate)
        {
            if (Student.SentVotes == null)
            {
                Student.SentVotes = new List<CandidateModel>
                {
                    Candidate
                };
                Student.TotalVotesApplied = "1";

                Candidate.TotalVotes = "1";
                Candidate.LastVoteReceived = GetVoteTimeStamp();

                FireStore.UpdateStudent(Student).Wait();
                FireStore.UpdateCandidate(Candidate).Wait();
            }
            else
            {
                if (Student.SentVotes.Any(c => c.Id == Candidate.Id)) //Unvote Operation
                {
                    Student.SentVotes.RemoveAll(c => c.Id == Candidate.Id);
                    Student.TotalVotesApplied = (int.Parse(Student.TotalVotesApplied) - 1).ToString();

                    Candidate.TotalVotes = (int.Parse(Candidate.TotalVotes) - 1).ToString();
                    Candidate.LastVoteReceived = GetVoteTimeStamp();

                    FireStore.UpdateStudent(Student).Wait();
                    FireStore.UpdateCandidate(Candidate).Wait();
                }
                else //Vote Operation
                {
                    Student.SentVotes.Add(Candidate);
                    Student.TotalVotesApplied = (int.Parse(Student.TotalVotesApplied) + 1).ToString();

                    Candidate.TotalVotes = (int.Parse(Candidate.TotalVotes) + 1).ToString();
                    Candidate.LastVoteReceived = GetVoteTimeStamp();

                    FireStore.UpdateStudent(Student).Wait();
                    FireStore.UpdateCandidate(Candidate).Wait();
                }
            }
        }
        #endregion
    }
}