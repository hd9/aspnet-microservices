using Core.Models;
using MassTransit;
using NotificationSvc.Infrastructure;
using NotificationSvc.Infrastructure.Options;
using NotificationSvc.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Services
{
    public class MailSender : IConsumer<SendMail>
    {

        #region Attributes
        private readonly MailOptions cfg;
        private readonly DbLogger dbLogger;
        #endregion

        public MailSender(MailOptions cfg, DbLogger dbLogger)
        {
            this.cfg = cfg;
            this.dbLogger = dbLogger;
        }

        public async Task Consume(ConsumeContext<SendMail> context)
        {
            var smtp = new SmtpClient
            {
                Host = cfg.Host,
                Port = cfg.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(cfg.Username, cfg.Password)
            };

            var name = context.Message.Name;
            var email = context.Message.Email;

            var msg = new MailMessage
            {
                From = new MailAddress(cfg.Username, cfg.FromName),
                Subject = cfg.Subject,
                Body = FormatBody(name, email)
            };

            msg.To.Add(cfg.To);

            await smtp.SendMailAsync(msg);
            await dbLogger.Log(name, email, 'E');
        }

        private string FormatBody(string name, string email)
        {
            var tpl = cfg.Template;
            return tpl.Replace("{Name}", name).Replace("{Email}", email);
        }

    }
}
