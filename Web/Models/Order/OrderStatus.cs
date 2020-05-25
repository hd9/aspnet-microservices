namespace Web.Models.Order
{
    public enum OrderStatus
    {
        New,
        Submitted,
        PaymentApproved,
        PaymentDeclined,
        WaitingShipping,
        Shipped,
        Delivered,
        Complete,
        Cancelled
    }
}