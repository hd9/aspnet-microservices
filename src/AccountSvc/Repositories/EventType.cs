using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSvc.Repositories
{
    /// <summary>
    /// Defines the events that can be registered on the event_type table
    /// </summary>
    public enum EventType
    {
        Login,
        AccountCreated,
        AccountUpdated,
        AccountClosed,
        PasswordCreated,
        PasswordUpdated,
        PasswordReset,
        ForgotPassword,
        AddressCreated,
        AddressUpdated,
        AddressRemoved,
        AddressSetDefault,
        PaymentInfoCreated,
        PaymentInfoUpdated,
        PaymentInfoRemoved,
        PaymentInfoSetDefault,
    }
}
