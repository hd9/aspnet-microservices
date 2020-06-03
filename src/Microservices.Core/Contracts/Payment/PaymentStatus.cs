namespace Microservices.Core.Contracts.Payment
{
    public enum PaymentStatus
    {
        Pending,
        Authorized,
        Declined,
        Cancelled,
        Refunded
    }
}