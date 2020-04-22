using Web.Models;

namespace Web.Services
{
    public interface INewsletterSvc
    {
        void Signup(NewsletterSignUp signup);
    }
}