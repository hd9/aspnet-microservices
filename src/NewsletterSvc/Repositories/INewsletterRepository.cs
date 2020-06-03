using NewsletterSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsletterSvc.Repositories
{
    public interface INewsletterRepository
    {
        Task Insert(Signup s);
        Task<List<Signup>> GetSignups(int recs);
    }
}