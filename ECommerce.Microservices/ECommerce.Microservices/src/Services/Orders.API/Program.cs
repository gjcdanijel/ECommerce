using Microsoft.EntityFrameworkCore;
using Orders.API.Data;
using Orders.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseInMemoryDatabase("OrdersDb"));

builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

builder.Services.AddHttpClient<CatalogHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Catalog"] ?? "http://localhost:5001");
});

builder.Services.AddHttpClient<CustomersHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Customers"] ?? "http://localhost:5002");
});

builder.Services.AddScoped<OrderService>();
builder.Services.AddHostedService<OrdersEventHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
