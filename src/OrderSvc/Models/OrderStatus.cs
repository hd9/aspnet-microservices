namespace OrderSvc.Models
{
    public enum OrderStatus
    {
        New,
        Submitted,
        PaymentApproved,
        PaymentDeclined,
        WaitingShipping,
        Shipped,
        Complete,
        Cancelled
    }
}