using Core.Events.Base;
using System;

namespace Core.Events.Newsletter
{
    public class AccountCreated : EventBase
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
