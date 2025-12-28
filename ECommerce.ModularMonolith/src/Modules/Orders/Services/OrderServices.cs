using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Orders.Contracts;
using ECommerce.Modules.Orders.Models;
using ECommerce.Modules.Catalog.Contracts;
using ECommerce.Modules.Customers.Contracts;

namespace ECommerce.Modules.Orders.Services;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly OrdersDbContext _context;

    public GetOrderByIdHandler(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.OrderId);
        if (order == null)
            return null;

        return new OrderDto(order.Id, order.CustomerId, order.ProductId, order.Quantity, 
            order.TotalAmount, order.Status, order.CreatedAt);
    }
}

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly OrdersDbContext _context;

    public UpdateOrderStatusHandler(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.OrderId);
        if (order == null)
            return false;

        order.Status = request.NewStatus;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class OrderService
{
    private readonly OrdersDbContext _context;
    private readonly IMediator _mediator;

    public OrderService(OrdersDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order> CreateAsync(int customerId, int productId, int quantity)
    {
        var customerExists = await _mediator.Send(new CustomerExistsQuery(customerId));
        if (!customerExists)
            throw new InvalidOperationException($"Kupac sa ID {customerId} ne postoji.");

        var product = await _mediator.Send(new GetProductByIdQuery(productId));
        if (product == null)
            throw new InvalidOperationException($"Proizvod sa ID {productId} ne postoji.");

        if (product.StockQuantity < quantity)
            throw new InvalidOperationException($"Nedovoljna količina na stanju. Dostupno: {product.StockQuantity}");

        var reserved = await _mediator.Send(new ReserveStockCommand(productId, quantity));
        if (!reserved)
            throw new InvalidOperationException("Nije moguće rezervisati traženu količinu.");

        var order = new Order
        {
            CustomerId = customerId,
            ProductId = productId,
            Quantity = quantity,
            TotalAmount = product.Price * quantity,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        await _mediator.Publish(new OrderCreatedEvent(order.Id, order.TotalAmount));

        return order;
    }
}
