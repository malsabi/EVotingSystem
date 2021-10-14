using EVotingSystem.Models;

namespace EVotingSystem.Helpers
{
    public class StudentHelper
    {
        /// <summary>
        /// Takes a StudentModel to extract the information and create a Statistical information.
        /// </summary>
        /// <param name="Student">Represents the Student Model</param>
        /// <returns>Returns Statistical information of the student</returns>
        public static StudentStatistics GetStudentStatistics(StudentModel Student)
        {
            StudentStatistics studentStatistics = new StudentStatistics();

            if (string.IsNullOrEmpty(Student.TotalVotesApplied) == false && Student.SentVotes.Count > 0)
            {
                int TotalVotes = int.Parse(Student.TotalVotesApplied);
                int MaleVotes = 0, FemaleVotes = 0;

                foreach (CandidateModel CM in Student.SentVotes)
                {
                    if (CM.Gender.Equals("Male"))
                    {
                        MaleVotes++;
                    }
                    else
                    {
                        FemaleVotes++;
                    }
                }

                double PercentageMaleVotes = MaleVotes / (double)TotalVotes * 100.0;
                double PercentageFemaleVotes = FemaleVotes / (double)TotalVotes * 100.0;

                studentStatistics.TotalVotes = TotalVotes.ToString();
                studentStatistics.PercentageMalesTotalVotes = PercentageMaleVotes.ToString();
                studentStatistics.PercentageFemalesTotalVotes = PercentageFemaleVotes.ToString();
            }

            return studentStatistics;
        }
    }
}