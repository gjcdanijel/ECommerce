using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Shared;
using ECommerce.Modules.Catalog.Services;

namespace ECommerce.Modules.Catalog;

public class CatalogModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseInMemoryDatabase("CatalogDb"));

        services.AddScoped<ProductService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CatalogModule).Assembly));
    }
}
