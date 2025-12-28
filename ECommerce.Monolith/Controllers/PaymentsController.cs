using Microsoft.AspNetCore.Mvc;
using ECommerce.Monolith.Models;
using ECommerce.Monolith.Services;

namespace ECommerce.Monolith.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetById(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<Payment>> GetByOrderId(int orderId)
    {
        var payment = await _paymentService.GetByOrderIdAsync(orderId);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> ProcessPayment(ProcessPaymentRequest request)
    {
        try
        {
            var payment = await _paymentService.ProcessPaymentAsync(request.OrderId, request.Method);
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class ProcessPaymentRequest
{
    public int OrderId { get; set; }
    public PaymentMethod Method { get; set; }
}
