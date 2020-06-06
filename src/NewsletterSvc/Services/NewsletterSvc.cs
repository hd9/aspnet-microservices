using Microservices.Core.Contracts.Newsletter;
using Microservices.Core.Contracts.Notification;
using Microservices.Core.Infrastructure.Extensions;
using Microservices.Core.Infrastructure.Options;
using MassTransit;
using NewsletterSvc.Models;
using NewsletterSvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsletterSvc.Services
{
    public class NewsletterSvc : INewsletterSvc
    {
        readonly INewsletterRepository _repo;
        readonly IBusControl _bus;
        readonly EmailTemplate _mailOptions;

        public NewsletterSvc(INewsletterRepository repo, IBusControl bus, EmailTemplate mailOptions)
        {
            _repo = repo;
            _bus = bus;
            _mailOptions = mailOptions;
        }

        public async Task<IList<Signup>> GetSignups(int recs = 10)
        {
            return await _repo.GetSignups(recs);
        }

        public async Task RegistrerSignup(Signup s)
        {
            if (s == null)
                return;

            await _repo.Insert(s);

            await _bus.Publish(
                new SendMail
                {
                    ToName = s.Name,
                    FromName = _mailOptions.FromName,
                    Email = s.Email,
                    Subject = _mailOptions.Subject,
                    Body = _mailOptions.Body.FormatWith(s.Name)
                });

            await _bus.Publish(
                new NewsletterSubscribed {
                    Name = s.Name,
                    Email = s.Email
                });
        }
    }
}
