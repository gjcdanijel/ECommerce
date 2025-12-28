using Microsoft.EntityFrameworkCore;
using Payments.API.Data;
using Payments.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseInMemoryDatabase("PaymentsDb"));

builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

builder.Services.AddHttpClient<OrdersHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Orders"] ?? "http://localhost:5003");
});

builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
