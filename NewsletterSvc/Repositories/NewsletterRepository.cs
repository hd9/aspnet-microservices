using Core.Models.Events;
using MassTransit;
using NewsletterSvc.Infrastructure;
using NewsletterSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsletterSvc.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private readonly IMongoClient db;

        public NewsletterRepository(IMongoClient db)
        {
            this.db = db;
        }

        public async Task<IList<Signup>> GetAll()
        {
            return await db.GetAll<Signup>();
        }

        public async Task RegistrerSignup(Signup s)
        {
            await db.Insert(s);
        }
    }
}
