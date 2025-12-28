using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Shared;
using ECommerce.Modules.Payments.Services;

namespace ECommerce.Modules.Payments;

public class PaymentsModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<PaymentsDbContext>(options =>
            options.UseInMemoryDatabase("PaymentsDb"));

        services.AddScoped<PaymentService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PaymentsModule).Assembly));
    }
}
