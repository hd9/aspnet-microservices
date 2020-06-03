namespace Microservices.Core.Contracts.Shipping
{
    public enum ShippingStatus
    {
        Pending,
        Delivered,
        Declined,
        Cancelled,
        Invalid,
        Error
    }
}