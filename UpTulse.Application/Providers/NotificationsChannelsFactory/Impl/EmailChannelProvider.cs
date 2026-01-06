using System.Net;
using System.Net.Mail;

using UpTulse.Application.Models;
using UpTulse.Application.Providers.NotificationsFactory;
using UpTulse.DataAccess.EnvironmentVariables;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Impl
{
    public class EmailChannelProvider : INotificationChannelProvider
    {
        private readonly string _smtpPass;
        private readonly int _smtpPort;
        private readonly List<string> _smtpSendToList;
        private readonly string _smtpServer;
        private readonly string _smtpUser;

        public EmailChannelProvider()
        {
            _smtpServer = Environment.GetEnvironmentVariable(NotificationsEnv.SMTP_HOST) ?? string.Empty;
            _smtpPort = int.TryParse(Environment.GetEnvironmentVariable(NotificationsEnv.SMTP_PORT), out var port) ? port : 587;
            _smtpUser = Environment.GetEnvironmentVariable(NotificationsEnv.SMTP_USER) ?? string.Empty;
            _smtpPass = Environment.GetEnvironmentVariable(NotificationsEnv.SMTP_PASS) ?? string.Empty;
            _smtpSendToList = [.. (Environment.GetEnvironmentVariable(NotificationsEnv.SMTP_SEND_TO) ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        }

        public async Task<bool> SendNotification(NotificationContext context)
        {
            var sendIsSuccess = false;

            try
            {
                var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = context.Subject,
                    Body = context.Body,
                    IsBodyHtml = true
                };

                foreach (var recipient in _smtpSendToList)
                {
                    mail.To.Add(recipient);
                }

                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception)
            {
                sendIsSuccess = false;
            }

            return sendIsSuccess;
        }
    }
}