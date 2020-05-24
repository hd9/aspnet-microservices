namespace HildenCo.Core.Contracts.Payment
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