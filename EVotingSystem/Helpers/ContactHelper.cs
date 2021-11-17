using EVotingSystem.Models.Contact;
using System.Net.Mail;

namespace EVotingSystem.Helpers
{
    public class ContactHelper
    {
        /// <summary>
        /// Sends a contact message to company
        /// </summary>
        /// <param name="Contact">Represents user contact model</param>
        public static void SendUserContact(ContactModel Contact)
        {
            const string CompanyEmail = "evotingsystemuae@gmail.com";
            const string CompanyPassword = "Evoting@uae@1";

            string UserName = string.Format("Name: {0}", string.Concat(Contact.FirstName, " ", Contact.LastName));
            string UserPhone = string.Format("Phone Number: {0}", Contact.PhoneNumber);
            string UserComment = string.Format("Comment:{0}{1}", "\n", Contact.Comment);
            string MessageBody = string.Concat(UserName, "\n", UserPhone, "\n", UserComment);
            MailMessage message = new MailMessage(CompanyEmail, Contact.EmailAddress, "Contact Feedback", MessageBody);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(CompanyEmail, CompanyPassword)
            };
            client.Send(message);
        }
    }
}