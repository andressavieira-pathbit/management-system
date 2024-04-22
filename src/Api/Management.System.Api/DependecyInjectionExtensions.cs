using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json.Serialization;

namespace Management.System.Api;

public static class DependecyInjectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiControllers();
        services.AddApiDocs();

        return services;

    }

    public static IApplicationBuilder UseApiServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    } 

    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    public static IServiceCollection AddApiDocs(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        { 
            options.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return [api.GroupName];
                }

                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return [controllerActionDescriptor.ControllerName];
                }

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });

            options.DocInclusionPredicate((name, api) => true);
            options.CustomSchemaIds(type => type.ToString());
        });

        return services;
    }
}

