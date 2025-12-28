using ECommerce.Modules.Catalog;
using ECommerce.Modules.Customers;
using ECommerce.Modules.Orders;
using ECommerce.Modules.Payments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var catalogModule = new CatalogModule();
var customersModule = new CustomersModule();
var ordersModule = new OrdersModule();
var paymentsModule = new PaymentsModule();

catalogModule.RegisterServices(builder.Services);
customersModule.RegisterServices(builder.Services);
ordersModule.RegisterServices(builder.Services);
paymentsModule.RegisterServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var catalogContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    catalogContext.Database.EnsureCreated();
    
    var customersContext = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
    customersContext.Database.EnsureCreated();
    
    var ordersContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    ordersContext.Database.EnsureCreated();
    
    var paymentsContext = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
    paymentsContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
