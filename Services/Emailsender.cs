using System.Net.Mail;
using cvweb.Services.Intefaces;

namespace cvweb.Services;

public class Emailsender : IEmailsender
{
    public async Task<bool> SendMail(string to, string subject, string body)
    {
        string smtp = "smtp.gmail.com";
        string from = "persiancode.info@gmail.com";
        string emailPassword = "fsqprgtuuhezqiyt"; // app password
        string displayName = "پرشین کد";
        int emailPort = 587; // domain smtp => 25

        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtp);
            mail.From = new MailAddress(from, displayName);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = emailPort;
            SmtpServer.EnableSsl = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential(from, emailPassword);
            SmtpServer.Send(mail);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}