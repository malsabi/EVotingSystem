using System;
using System.Net.Mail;

namespace EVotingSystem.Helpers
{
    public class VerificationHelper
    {
        /// <summary>
        /// Creates a random code with a given length
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Returns a randomized with a given length</returns>
        public static string GetVerificationCode(int length)
        {
            char[] chArray = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(1, chArray.Length);
                if (!str.Contains(chArray.GetValue(index).ToString()))
                {
                    str += chArray.GetValue(index);
                }
                else
                {
                    i--;
                }
            }
            return str;
        }

        /// <summary>
        /// Sends a code to the student by providing the email.
        /// </summary>
        /// <param name="FirstName">Represents the student first name</param>
        /// <param name="LastName">Represents the student last name</param>
        /// <param name="Email">Represents the student email address</param>
        /// <param name="Code">Represents the confirmation code</param>
        public static void SendCode(string FirstName, string LastName, string Email, string Code)
        {
            const string CompanyEmail = "evotingsystemuae@gmail.com";
            const string CompanyPassword = "Evoting@uae@1";

            string SirFullName = string.Format("Full Name: {0}", string.Concat(FirstName, " ", LastName));
            string ConfirmationCode = string.Format("Confirmation Code: {0}", Code);
            string Message = string.Concat(SirFullName, "\n", ConfirmationCode);

            MailMessage mailMessage = new MailMessage(CompanyEmail, Email, "Confirmation Code", Message);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(CompanyEmail, CompanyPassword)
            };
            client.Send(mailMessage);
        }
    }
}