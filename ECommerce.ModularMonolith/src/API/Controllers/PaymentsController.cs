using Microsoft.AspNetCore.Mvc;
using ECommerce.Modules.Payments.Models;
using ECommerce.Modules.Payments.Services;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
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
