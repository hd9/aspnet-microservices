using MassTransit;
using Microservices.Core.Contracts.Notification;
using Microservices.Core.Infrastructure.Extensions;
using Microservices.Core.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using NotificationSvc.Repositories;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Consumers
{
    public class SendMailConsumer : IConsumer<SendMail>
    {

        #region Attributes
        readonly SmtpOptions _smtpOptions;
        readonly ILogger<SendMailConsumer> _logger;
        readonly INotificationRepository _repo;
        #endregion

        public SendMailConsumer(
            INotificationRepository repo, 
            SmtpOptions smtpOptions, 
            ILogger<SendMailConsumer> logger)
        {
            _repo = repo;
            _smtpOptions = smtpOptions;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendMail> context)
        {
            var smtpClient = new SmtpClient
            {
                Host = _smtpOptions.Host,
                Port = _smtpOptions.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password)
            };

            var msg = context.Message;
            var mail = new MailMessage
            {
                From = new MailAddress(_smtpOptions.FromEmail, _smtpOptions.FromName),
                Subject = msg.Subject,
                Body = msg.Body
            };

            if(!_smtpOptions.EmailOverride.HasValue() &&
                !msg.ToEmail.HasValue())
            {
                _logger.LogError("Missing target email. Did you forget to send or configure it?");
                return;
            }

            // if set, overrides with smtpOptions.EmailOverride
            mail.To.Add(_smtpOptions.EmailOverride ?? msg.ToEmail);

            _logger.LogInformation($"Logging event on the db...");
            await _repo.Insert(msg.ToName, msg.ToEmail, 'E');

            _logger.LogInformation($"Sending email to \"{msg.ToName} <{msg.ToEmail}>\"...");
            await smtpClient.SendMailAsync(mail);
            _logger.LogInformation("Email successfully sent!");
        }
    }
}
