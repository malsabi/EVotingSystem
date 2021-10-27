namespace EVotingSystem.Models.Student
{
    public class StudentStatistics
    {
        public string TotalVotes { get; set; }
        public string PercentageMalesTotalVotes { get; set; }
        public string PercentageFemalesTotalVotes { get; set; }

        public StudentStatistics()
        {
            TotalVotes = "0";
            PercentageMalesTotalVotes = "0 %";
            PercentageFemalesTotalVotes = "0 %";
        }
    }
}