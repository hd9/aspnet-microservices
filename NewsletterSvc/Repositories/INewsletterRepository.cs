using NewsletterSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsletterSvc.Repositories
{
    public interface INewsletterRepository
    {
        Task<IList<Signup>> GetAll();
        Task RegistrerSignup(Signup s);
    }
}