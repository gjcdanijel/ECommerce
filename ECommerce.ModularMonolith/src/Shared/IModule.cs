using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared;

public interface IModule
{
    void RegisterServices(IServiceCollection services);
}
