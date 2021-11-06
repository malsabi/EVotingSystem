using EVotingSystem.Constants;
using EVotingSystem.DataBase;
using EVotingSystem.Models.Candidate;
using EVotingSystem.Models.Student;
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
        public bool ApplyVote(StudentModel Student, CandidateModel Candidate)
        {
            Student.DecryptProperties();
            Candidate.EncryptProperties();

            if (int.Parse(Student.TotalVotesApplied) >= Config.MaximumStudentVotes)
            {
                return false;
            }
            else
            {
                StudentVoteModel StudentVote = new StudentVoteModel()
                {
                    Id = Student.StudentId,
                    Gender = Student.Gender
                };
                StudentVote.EncryptProperties();

                //Increment the total votes the user has applied, the maximum is 2 votes for each student
                Student.TotalVotesApplied = (int.Parse(Student.TotalVotesApplied) + 1).ToString();

                //Extract the candidate to be voted on, from the CandidateVote Entity.
                CandidateVoteModel CandidateVote = FireStore.GetCandidateVote(Candidate.Id).Result;
             
                if (CandidateVote == null)
                {
                    return false;
                }
                else
                {
                    //Initialize the StudentVote list, it is null if it was the first vote for the candidate.
                    if (CandidateVote.StudentVoteCollection == null)
                    {
                        //Decrypt the candidate vote
                        CandidateVote.DecryptProperties();
                        //Initialize the StudentVote collection
                        CandidateVote.StudentVoteCollection = new List<StudentVoteModel>();
                        //Apply vote by incrementing the total votes and adding the student who applied the vote.
                        CandidateVote.TotalVotes = (int.Parse(CandidateVote.TotalVotes) + 1).ToString();
                        CandidateVote.StudentVoteCollection.Add(StudentVote);
                        //Encrypt the candidate vote back again after finishing from editing.
                        CandidateVote.EncryptProperties();
                        //Update the Last Vote Received for the candidate.
                        Candidate.LastVoteReceived = DateTime.Now.ToLongDateString().ToString();
                    }
                    else
                    {
                        //Prevent the student to vote to the same candidate twice.
                        if (CandidateVote.StudentVoteCollection.Any(sv => sv.Id.Equals(StudentVote.Id)))
                        {
                            return false;
                        }
                        else
                        {
                            //Decrypt the candidate vote to edit the fields
                            CandidateVote.DecryptProperties();
                            //Apply vote by incrementing the total votes and adding the student who applied the vote.
                            CandidateVote.TotalVotes = (int.Parse(CandidateVote.TotalVotes) + 1).ToString();
                            CandidateVote.StudentVoteCollection.Add(StudentVote);
                            //Encrypt the candidate vote back again after finishing from editing.
                            CandidateVote.EncryptProperties();
                            //Update the Last Vote Received for the candidate.
                            Candidate.LastVoteReceived = DateTime.Now.ToLongDateString().ToString();
                        }
                    }

                    //Encrypt the student back
                    Student.EncryptProperties();
                    //Update Student Entity
                    FireStore.UpdateStudent(Student).Wait();
                    //Update Candidate Entity
                    FireStore.UpdateCandidate(Candidate).Wait();
                    //Update CandidateVote Entity
                    FireStore.UpdateCandidateVote(CandidateVote).Wait();

                    return true;
                }
            }
        }
        #endregion

        #region "Static Methods"
        public static List<CandidateModel> GetStudentCandidateVotes(StudentModel Student)
        {
            FireStoreManager FireStore = new FireStoreManager();
            List<CandidateModel> Candidates = FireStore.GetAllCandidates().Result;
            List<CandidateModel> StudentCanddiateVotes = new List<CandidateModel>();
            foreach (CandidateModel Candidate in Candidates)
            {
                CandidateVoteModel CandidateVote = FireStore.GetCandidateVote(Candidate.Id).Result;
                if (CandidateVote.StudentVoteCollection != null)
                {
                    if (CandidateVote.StudentVoteCollection.Any(cv => cv.Id.Equals(Student.StudentId)))
                    {
                        StudentCanddiateVotes.Add(Candidate);
                    }
                }
            }
            return StudentCanddiateVotes;
        }
        public static bool IsCandidateVoted(CandidateModel Candidate, StudentModel Student)
        {
            if (Student == null || Candidate == null)
            {
                return false;
            }
            else
            {
                Candidate.EncryptProperties();
                CandidateVoteModel CandidateVote = new FireStoreManager().GetCandidateVote(Candidate.Id).Result;
                Candidate.DecryptProperties();
                if (CandidateVote.StudentVoteCollection != null)
                {
                    if (CandidateVote.StudentVoteCollection.Any(cv => cv.Id.Equals(Student.StudentId)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}