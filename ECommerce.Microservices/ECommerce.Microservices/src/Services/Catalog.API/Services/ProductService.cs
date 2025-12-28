using Microsoft.EntityFrameworkCore;
using Catalog.API.Data;
using Catalog.API.Models;
using ECommerce.Shared.Events;

namespace Catalog.API.Services;

public class ProductService
{
    private readonly CatalogDbContext _context;
    private readonly IMessageBus _messageBus;

    public ProductService(CatalogDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> ReserveStockAsync(int productId, int quantity, int orderId)
    {
        var product = await _context.Products.FindAsync(productId);
        
        if (product == null || product.StockQuantity < quantity)
        {
            var reason = product == null ? "Proizvod ne postoji" : "Nedovoljna količina";
            await _messageBus.PublishAsync("stock-reservation-failed", 
                new StockReservationFailedEvent(productId, orderId, reason));
            return false;
        }

        product.StockQuantity -= quantity;
        await _context.SaveChangesAsync();

        await _messageBus.PublishAsync("stock-reserved", 
            new StockReservedEvent(productId, quantity, orderId));

        return true;
    }
}
