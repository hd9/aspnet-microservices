using Core.Models;
using MassTransit;
using NotificationSvc.Infrastructure;
using NotificationSvc.Infrastructure.Options;
using NotificationSvc.Models;
using Svc = NotificationSvc.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using NotificationSvc.Commands;
using NotificationSvc.Repositories;

namespace NotificationSvc.Consumers
{
    public class MailSenderConsumer : IConsumer<SendMail>
    {

        #region Attributes
        private readonly MailOptions _cfg;
        private readonly INotificationRepository _repo;
        #endregion

        public MailSenderConsumer(MailOptions cfg, INotificationRepository repo)
        {
            _cfg = cfg;
            _repo = repo;
        }

        public async Task Consume(ConsumeContext<SendMail> context)
        {
            var smtpClient = new SmtpClient
            {
                Host = _cfg.Host,
                Port = _cfg.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_cfg.Username, _cfg.Password)
            };

            var name = context.Message.Name;
            var email = context.Message.Email;

            var msg = new MailMessage
            {
                From = new MailAddress(_cfg.Username, _cfg.FromName),
                Subject = _cfg.Subject,
                Body = FormatBody(name, email)
            };

            msg.To.Add(_cfg.To);

            await smtpClient.SendMailAsync(msg);
            await _repo.LogNotification(name, email, 'E');
        }

        private string FormatBody(string name, string email)
        {
            var tpl = _cfg.Template;
            return tpl.Replace("{Name}", name).Replace("{Email}", email);
        }

    }
}
