using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface INewsletterProxy
    {
        Task Signup(NewsletterSignUp signup);
    }
}