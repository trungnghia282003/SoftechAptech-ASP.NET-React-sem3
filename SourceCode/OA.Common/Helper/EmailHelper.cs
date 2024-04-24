using System.Net;
using System.Net.Mail;

namespace OA.Common.Helper
{
    public static class EmailHelper
    {
        public async static Task<bool> SendMailAsync(string email, string body, string subject)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.Subject = subject;
                mail.From = new MailAddress("kynkuty915@gmail.com");
                mail.To.Add(email);
                mail.Body = body;
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                NetworkCredential netCre = new NetworkCredential("kynkuty915@gmail.com", "unwq plnw awnj aqjx");
                smtp.Credentials = netCre;
                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}
