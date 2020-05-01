using Core.Models.Events;
using MassTransit;
using NewsletterSvc.Models;
using NewsletterSvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsletterSvc.Services
{
    public class NewsletterSvc : INewsletterSvc
    {
        private readonly INewsletterRepository _repo;
        private readonly IBusControl _bus;

        public NewsletterSvc(INewsletterRepository repo, IBusControl bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task<IList<Signup>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task RegistrerSignup(Signup s)
        {
            if (s == null) return;

            await _repo.RegistrerSignup(s);
            await _bus.Publish(new NewsletterSubscribed { Name = s.Name, Email = s.Email });
        }
    }
}
