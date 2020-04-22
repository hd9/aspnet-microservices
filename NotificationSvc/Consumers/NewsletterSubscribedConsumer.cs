using Core;
using Core.Models;
using Core.Models.Events;
using MassTransit;
using NotificationSvc.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSvc.Consumers
{
    public class NewsletterSubscribedConsumer : IConsumer<NewsletterSubscribed>
    {
        public async Task Consume(ConsumeContext<NewsletterSubscribed> context)
        {
            await Console.Out.WriteLineAsync(context.Message.ToString());

            // todo :: do your stuff


            await context.Publish(new SendMail { Email = context.Message.Email, Name = context.Message.Name });
        }
    }
}
