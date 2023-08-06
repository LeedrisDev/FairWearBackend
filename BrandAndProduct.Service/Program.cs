using BrandAndProduct.Service.Config;
using BrandAndProduct.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Validate required environment variables
EnvironmentValidator.ValidateRequiredVariables();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

DependencyInjectionConfiguration.Configure(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<BrandService>();
app.MapGrpcService<ProductService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();