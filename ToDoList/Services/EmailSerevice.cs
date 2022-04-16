using MimeKit;
using MailKit.Net.Smtp;

namespace ToDoList.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string subject, string body, params string[] sendersEmail)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ToDoList", "no-reply@sana-to-do-list.herokuapp.com"));
            foreach(var email in sendersEmail)
                emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body,
            };

            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync(Environment.GetEnvironmentVariable("SmtpServer"), int.Parse(Environment.GetEnvironmentVariable("Port")));
                await client.AuthenticateAsync(Environment.GetEnvironmentVariable("Username"), Environment.GetEnvironmentVariable("Password"));
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
