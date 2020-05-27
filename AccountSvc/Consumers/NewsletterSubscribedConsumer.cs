using AccountSvc.Services;
using HildenCo.Core.Contracts.Newsletter;
using MassTransit;
using System.Threading.Tasks;

namespace AccountSvc.Consumers
{
    public class NewsletterSubscribedConsumer : IConsumer<NewsletterSubscribed>
    {
        readonly IAccountSvc _svc;

        public NewsletterSubscribedConsumer(IAccountSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<NewsletterSubscribed> context)
        {
            await _svc.SubscribeToNewsletter(context.Message.Email);
        }
    }
}
