using HildenCo.Core.Contracts.Base;

namespace HildenCo.Core.Contracts.Newsletter
{
    public class NewsletterSubscribed : EventBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public override string ToString() => 
            $"[NewsletterSubscription] \"{Name}\" <{Email}>";
    }
}
