using Management.System.Application.Management.Services;
using Management.System.Application.Services;
using Management.System.Domain.Management.ExternalApis;
using Management.System.Domain.Management.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Application;

[ExcludeFromCodeCoverage]
public static class DependecyInjectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    { 
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IZipCodeService, ZipCodeService>();

        return services;
    }
}
