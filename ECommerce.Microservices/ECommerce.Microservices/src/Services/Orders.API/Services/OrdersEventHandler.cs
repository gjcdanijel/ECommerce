using Orders.API.Models;
using ECommerce.Shared.Events;

namespace Orders.API.Services;

public class OrdersEventHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageBus _messageBus;

    public OrdersEventHandler(IServiceProvider serviceProvider, IMessageBus messageBus)
    {
        _serviceProvider = serviceProvider;
        _messageBus = messageBus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBus.Subscribe<StockReservedEvent>("stock-reserved", HandleStockReserved);
        _messageBus.Subscribe<StockReservationFailedEvent>("stock-reservation-failed", HandleStockReservationFailed);
        _messageBus.Subscribe<OrderPaidEvent>("order-paid", HandleOrderPaid);
        
        return Task.CompletedTask;
    }

    private async Task HandleStockReserved(StockReservedEvent evt)
    {
        using var scope = _serviceProvider.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
        
        await orderService.UpdateStatusAsync(evt.OrderId, OrderStatus.StockReserved);
    }

    private async Task HandleStockReservationFailed(StockReservationFailedEvent evt)
    {
        using var scope = _serviceProvider.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
        
        await orderService.UpdateStatusAsync(evt.OrderId, OrderStatus.Cancelled);
    }

    private async Task HandleOrderPaid(OrderPaidEvent evt)
    {
        using var scope = _serviceProvider.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
        
        await orderService.UpdateStatusAsync(evt.OrderId, OrderStatus.Confirmed);
    }
}
