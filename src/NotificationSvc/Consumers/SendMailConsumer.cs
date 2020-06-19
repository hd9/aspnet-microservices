using Microservices.Core.Contracts.Notification;
using Microservices.Core.Infrastructure.Extensions;
using Microservices.Core.Infrastructure.Options;
using MassTransit;
using NotificationSvc.Repositories;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static Microservices.Core.Infrastructure.Extensions.ExceptionExtensions;
using Microsoft.Extensions.Logging;

namespace NotificationSvc.Consumers
{
    public class SendMailConsumer : IConsumer<SendMail>
    {

        #region Attributes
        readonly SmtpOptions _smtpOptions;
        readonly ILogger<SendMailConsumer> _logger;
        readonly INotificationRepository _repo;
        #endregion

        public SendMailConsumer(INotificationRepository repo, SmtpOptions smtpOptions, ILogger<SendMailConsumer> logger)
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
            var toName = msg.ToName;
            var email = msg.Email;

            var mail = new MailMessage
            {
                From = new MailAddress(_smtpOptions.Username, msg.FromName),
                Subject = msg.Subject,
                Body = msg.Body
            };

            Throw<ArgumentNullException>.If(
                !_smtpOptions.EmailOverride.HasValue() &&
                !msg.Email.HasValue(),
                "Missing target email. Did you forget to send or configure it?");

            mail.To.Add(_smtpOptions.EmailOverride ?? msg.Email);

            _logger.LogInformation($"Logging event on the db...");
            await _repo.Insert(toName, email, 'E');

            _logger.LogInformation($"Sending email to {toName} <{email}>...");
            await smtpClient.SendMailAsync(mail);
            _logger.LogInformation("Email successfully sent!");
        }
    }
}
