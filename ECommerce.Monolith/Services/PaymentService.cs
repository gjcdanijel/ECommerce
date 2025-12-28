using Microsoft.EntityFrameworkCore;
using ECommerce.Monolith.Data;
using ECommerce.Monolith.Models;

namespace ECommerce.Monolith.Services;

public interface IPaymentService
{
    Task<Payment?> GetByIdAsync(int id);
    Task<Payment?> GetByOrderIdAsync(int orderId);
    Task<Payment> ProcessPaymentAsync(int orderId, PaymentMethod method);
}

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;

    public PaymentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment> ProcessPaymentAsync(int orderId, PaymentMethod method)
    {
        var order = await _context.Orders.FindAsync(orderId);
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

        order.Status = OrderStatus.Confirmed;

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        payment.Order = order;

        return payment;
    }
}
