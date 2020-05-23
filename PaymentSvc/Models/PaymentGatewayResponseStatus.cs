namespace PaymentSvc.Models
{
    public enum PaymentGatewayResponseStatus
    {
        Pending,
        Authorized,
        Declined,
        Invalid,
        Error
    }
}
