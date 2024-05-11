using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSetting
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("routec41v02@gmail.com", "bqfivjutajhfxgto");
            client.Send("routec41v02@gmail.com", email.Recipients, email.Subject, email.Body);
            //maryamgmal333 @gmail.com
        }
    }
}
