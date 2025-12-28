namespace ECommerce.Shared.Events;

public record OrderCreatedEvent(int OrderId, int CustomerId, int ProductId, int Quantity, decimal TotalAmount);

public record OrderPaidEvent(int OrderId, int PaymentId);

public record StockReservedEvent(int ProductId, int Quantity, int OrderId);

public record StockReservationFailedEvent(int ProductId, int OrderId, string Reason);
