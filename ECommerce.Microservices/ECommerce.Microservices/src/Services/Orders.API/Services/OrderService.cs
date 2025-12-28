using Microsoft.EntityFrameworkCore;
using Orders.API.Data;
using Orders.API.Models;
using ECommerce.Shared.Events;

namespace Orders.API.Services;

public class OrderService
{
    private readonly OrdersDbContext _context;
    private readonly CatalogHttpClient _catalogClient;
    private readonly CustomersHttpClient _customersClient;
    private readonly IMessageBus _messageBus;

    public OrderService(
        OrdersDbContext context,
        CatalogHttpClient catalogClient,
        CustomersHttpClient customersClient,
        IMessageBus messageBus)
    {
        _context = context;
        _catalogClient = catalogClient;
        _customersClient = customersClient;
        _messageBus = messageBus;
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
        var customerExists = await _customersClient.CustomerExistsAsync(customerId);
        if (!customerExists)
            throw new InvalidOperationException($"Kupac sa ID {customerId} ne postoji.");

        var product = await _catalogClient.GetProductAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Proizvod sa ID {productId} ne postoji.");

        if (product.StockQuantity < quantity)
            throw new InvalidOperationException($"Nedovoljna količina na stanju. Dostupno: {product.StockQuantity}");

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

        await _messageBus.PublishAsync("order-created",
            new OrderCreatedEvent(order.Id, customerId, productId, quantity, order.TotalAmount));

        return order;
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
