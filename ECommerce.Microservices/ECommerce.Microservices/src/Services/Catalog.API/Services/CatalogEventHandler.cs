using ECommerce.Shared.Events;

namespace Catalog.API.Services;

public class CatalogEventHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageBus _messageBus;

    public CatalogEventHandler(IServiceProvider serviceProvider, IMessageBus messageBus)
    {
        _serviceProvider = serviceProvider;
        _messageBus = messageBus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBus.Subscribe<OrderCreatedEvent>("order-created", HandleOrderCreated);
        return Task.CompletedTask;
    }

    private async Task HandleOrderCreated(OrderCreatedEvent evt)
    {
        using var scope = _serviceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        await productService.ReserveStockAsync(evt.ProductId, evt.Quantity, evt.OrderId);
    }
}
