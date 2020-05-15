using Core.Events.Base;
using System;

namespace Core.Events.Newsletter
{
    public class NewsletterSubscribed : EventBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public override string ToString() => 
            $"[NewsletterSubscription] \"{Name}\" <{Email}>";
    }
}
