using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Shared;
using ECommerce.Modules.Orders.Services;

namespace ECommerce.Modules.Orders;

public class OrdersModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<OrdersDbContext>(options =>
            options.UseInMemoryDatabase("OrdersDb"));

        services.AddScoped<OrderService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OrdersModule).Assembly));
    }
}
