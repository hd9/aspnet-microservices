using Microservices.Core.Contracts.Base;

namespace Microservices.Core.Contracts.Account
{
    public class AccountCreated : EventBase
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
