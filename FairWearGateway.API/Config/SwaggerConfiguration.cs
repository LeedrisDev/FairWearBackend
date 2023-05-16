using System.Reflection;
using Microsoft.OpenApi.Models;

namespace FairWearGateway.API.Config;

/// <summary>Static class that configures the Swagger documentation.</summary>
public static class SwaggerConfiguration
{
    /// <summary>Configures the Swagger documentation.</summary>
    /// <param name="services"> The service collection.</param>
    public static void Configure(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FairWear Gateway API",
                Version = "v1",
                Description = "API to handle any request from the frontend and redirect it to the correct microservice.",
                Contact = new OpenApiContact
                {
                    Name = "FairWear Group",
                    Email = "fairwear.group@gmail.com"
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}