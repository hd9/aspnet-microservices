namespace PaymentSvc.Models
{
    public enum PaymentStatus
    {
        Pending,
        Authorized,
        Declined,
        Cancelled,
        Refunded,
        Invalid,
        Error
    }
}