using Microsoft.EntityFrameworkCore;
using Payments.API.Data;
using Payments.API.Models;
using ECommerce.Shared.Events;

namespace Payments.API.Services;

public class PaymentService
{
    private readonly PaymentsDbContext _context;
    private readonly OrdersHttpClient _ordersClient;
    private readonly IMessageBus _messageBus;

    public PaymentService(PaymentsDbContext context, OrdersHttpClient ordersClient, IMessageBus messageBus)
    {
        _context = context;
        _ordersClient = ordersClient;
        _messageBus = messageBus;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment> ProcessPaymentAsync(int orderId, PaymentMethod method)
    {
        var order = await _ordersClient.GetOrderAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Narudžba sa ID {orderId} ne postoji.");

        var existingPayment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (existingPayment != null)
            throw new InvalidOperationException($"Plaćanje za narudžbu {orderId} već postoji.");

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = order.TotalAmount,
            Method = method,
            Status = PaymentStatus.Completed,
            ProcessedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        await _messageBus.PublishAsync("order-paid", new OrderPaidEvent(orderId, payment.Id));

        return payment;
    }
}
