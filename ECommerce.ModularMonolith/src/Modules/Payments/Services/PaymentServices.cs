using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Payments.Contracts;
using ECommerce.Modules.Payments.Models;
using ECommerce.Modules.Orders.Contracts;
using ECommerce.Modules.Orders.Models;

namespace ECommerce.Modules.Payments.Services;

public class GetPaymentByOrderIdHandler : IRequestHandler<GetPaymentByOrderIdQuery, PaymentDto?>
{
    private readonly PaymentsDbContext _context;

    public GetPaymentByOrderIdHandler(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentDto?> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == request.OrderId, cancellationToken);
        if (payment == null)
            return null;

        return new PaymentDto(payment.Id, payment.OrderId, payment.Amount, payment.Method, payment.Status);
    }
}

public class PaymentService
{
    private readonly PaymentsDbContext _context;
    private readonly IMediator _mediator;

    public PaymentService(PaymentsDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
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
        var order = await _mediator.Send(new GetOrderByIdQuery(orderId));
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

        await _mediator.Send(new UpdateOrderStatusCommand(orderId, OrderStatus.Confirmed));

        return payment;
    }
}
