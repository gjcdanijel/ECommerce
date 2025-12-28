using MediatR;
using ECommerce.Modules.Orders.Models;

namespace ECommerce.Modules.Orders.Contracts;

public record OrderDto(int Id, int CustomerId, int ProductId, int Quantity, decimal TotalAmount, OrderStatus Status, DateTime CreatedAt);

public record GetOrderByIdQuery(int OrderId) : IRequest<OrderDto?>;

public record UpdateOrderStatusCommand(int OrderId, OrderStatus NewStatus) : IRequest<bool>;

public record OrderCreatedEvent(int OrderId, decimal TotalAmount) : INotification;
