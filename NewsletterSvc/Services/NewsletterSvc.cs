using Core.Shared;
using MassTransit;
using NewsletterSvc.Infrastructure;
using NewsletterSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsletterSvc.Services
{
    public class NewsletterSvc : INewsletterSvc
    {
        private readonly IMongoClient db;
        private readonly IBusControl bus;

        public NewsletterSvc(IMongoClient db, IBusControl bus)
        {
            this.db = db;
            this.bus = bus;
        }

        public IList<Signup> GetAll()
        {
            return db.GetAll<Signup>();
        }

        public async Task RegistrerSignup(Signup s)
        {
            if (s == null) return;

            await db.Insert(s);
            await bus.Publish(new NewsletterSubscribed { Name = s.Name, Email = s.Email });
        }
    }
}
