using Web.Models;

namespace Web.Services
{
    public interface INewsletterProxy
    {
        void Signup(NewsletterSignUp signup);
    }
}