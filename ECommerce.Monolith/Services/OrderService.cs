using Microsoft.EntityFrameworkCore;
using ECommerce.Monolith.Data;
using ECommerce.Monolith.Models;

namespace ECommerce.Monolith.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateAsync(int customerId, int productId, int quantity);
}

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateAsync(int customerId, int productId, int quantity)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            throw new InvalidOperationException($"Kupac sa ID {customerId} ne postoji.");

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Proizvod sa ID {productId} ne postoji.");

        if (product.StockQuantity < quantity)
            throw new InvalidOperationException($"Nedovoljna količina na stanju. Dostupno: {product.StockQuantity}");

        product.StockQuantity -= quantity;

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

        order.Customer = customer;
        order.Product = product;

        return order;
    }
}
