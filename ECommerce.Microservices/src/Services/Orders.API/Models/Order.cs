namespace Orders.API.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum OrderStatus
{
    Pending,
    StockReserved,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}
