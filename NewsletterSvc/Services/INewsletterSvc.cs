using System.Collections.Generic;
using System.Threading.Tasks;
using NewsletterSvc.Models;

namespace NewsletterSvc.Services
{
    public interface INewsletterSvc
    {
        Task RegistrerSignup(Signup s);
        IList<Signup> GetAll();
    }
}