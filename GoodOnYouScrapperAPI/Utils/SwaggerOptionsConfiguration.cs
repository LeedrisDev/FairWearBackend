using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GoodOnYouScrapperAPI.Utils;

/// <summary>Configures the Swagger options</summary>
public class SwaggerOptionsConfiguration: IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    
    /// <summary>Constructor</summary>
    /// <param name="provider">The API version description provider.</param>
    public SwaggerOptionsConfiguration(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    
    /// <inheritdoc/>
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }
    
    /// <inheritdoc/>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }   
    
    /// <summary>Create information about the version of the API</summary>
    /// <param name="desc"></param>
    /// <returns>Information about the API</returns>
    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var info = new OpenApiInfo
        {
            Title = AppConstants.ApiName,
            Version = desc.ApiVersion.ToString()
        };

        if (desc.IsDeprecated)
            info.Description += $" {AppConstants.ApiVersionDeprecatedDescription}";

        return info;
    }
}