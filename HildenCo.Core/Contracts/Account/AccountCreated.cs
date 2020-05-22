using HildenCo.Core.Contracts.Base;

namespace HildenCo.Core.Contracts.Account
{
    public class AccountCreated : EventBase
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
