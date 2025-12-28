namespace Payments.API.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime ProcessedAt { get; set; }
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    BankTransfer
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}
