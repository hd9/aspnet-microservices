using AccountSvc.Services;
using Core.Commands;
using Core.Events;
using Core.Events.Newsletter;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AccountSvc.Consumers
{
    public class NewsletterSubscribedConsumer : IConsumer<NewsletterSubscribed>
    {
        private readonly IAccountSvc _svc;

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
