using MediatR;

namespace ECommerce.Modules.Catalog.Contracts;

public record ProductDto(int Id, string Name, decimal Price, int StockQuantity);

public record GetProductByIdQuery(int ProductId) : IRequest<ProductDto?>;

public record ReserveStockCommand(int ProductId, int Quantity) : IRequest<bool>;
