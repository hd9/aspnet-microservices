using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSvc.Repositories
{
    /// <summary>
    /// Defines the events that can be registered on the event_type table
    /// </summary>
    public enum EventType
    {
        OrderCreated = 1,
        OrderUpdated,
        OrderCancelled,
        OrderApproved,
        OrderShipped,
        OrderClosed,
        PaymentSubmitted = 10,
        PaymentRequested,
        PaymentUpdated,
        PaymentAuthorized,
        PaymentDeclined,
        ShippingInfoSubmitted = 20,
        ShippingInfoUpdated,
        ShippingInfoRemoved
    }
}
