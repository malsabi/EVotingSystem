using System;
namespace EVotingSystem.Helpers
{
    public class DateHelper
    {
        public static bool IsDateValid(string Date)
        {
            try
            {
                return DateTime.TryParse(Date, out DateTime Result);
            }
            catch
            {
                return false;
            }
        }

        public static string GetCurrentDate()
        {
            try
            {
                return DateTime.Now.ToShortDateString();
            }
            catch
            {
                return "N/A";
            }
        }

        public static bool IsDateBeforeOrEqual(string dueDate)
        {
            try
            {
                if (IsDateValid(dueDate))
                {
                    DateTime CurrentDueDate = DateTime.Parse(dueDate);
                    return CurrentDueDate <= DateTime.Now;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}