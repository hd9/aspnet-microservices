namespace PaymentSvc.Models
{
    public enum PaymentGatewayResponseStatus
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
