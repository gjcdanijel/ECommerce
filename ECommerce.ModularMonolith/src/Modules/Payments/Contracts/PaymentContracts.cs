using MediatR;
using ECommerce.Modules.Payments.Models;

namespace ECommerce.Modules.Payments.Contracts;

public record PaymentDto(int Id, int OrderId, decimal Amount, PaymentMethod Method, PaymentStatus Status);

public record GetPaymentByOrderIdQuery(int OrderId) : IRequest<PaymentDto?>;
