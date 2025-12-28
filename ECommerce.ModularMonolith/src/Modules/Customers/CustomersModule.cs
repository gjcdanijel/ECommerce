using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Shared;
using ECommerce.Modules.Customers.Services;

namespace ECommerce.Modules.Customers;

public class CustomersModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<CustomersDbContext>(options =>
            options.UseInMemoryDatabase("CustomersDb"));

        services.AddScoped<CustomerService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CustomersModule).Assembly));
    }
}
