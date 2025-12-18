namespace cvweb.Services.Intefaces;

public interface IEmailsender
{
   public Task<bool> SendMail(string to, string subject, string body);
}