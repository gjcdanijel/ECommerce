using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Catalog.Contracts;
using ECommerce.Modules.Catalog.Models;

namespace ECommerce.Modules.Catalog.Services;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly CatalogDbContext _context;

    public GetProductByIdHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        if (product == null)
            return null;

        return new ProductDto(product.Id, product.Name, product.Price, product.StockQuantity);
    }
}

public class ReserveStockHandler : IRequestHandler<ReserveStockCommand, bool>
{
    private readonly CatalogDbContext _context;

    public ReserveStockHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        if (product == null || product.StockQuantity < request.Quantity)
            return false;

        product.StockQuantity -= request.Quantity;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ProductService
{
    private readonly CatalogDbContext _context;

    public ProductService(CatalogDbContext context)
    {
        _context = context;
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
}
