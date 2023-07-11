using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Config;
using FairWearGateway.API.Utils;
using Grpc.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Validate required environment variables
EnvironmentValidator.ValidateRequiredVariables();

// Dependency Injection
DependencyInjectionConfiguration.Configure(builder.Services);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddGrpcClient<BrandService.BrandServiceClient>(options =>
    {
        options.Address = new Uri(AppConstants.BrandAndProductServiceUrl);
    })
    .ConfigureChannel(channelOptions => { channelOptions.Credentials = ChannelCredentials.Insecure; });

// builder.Services.AddGrpcClient<BrandService.BrandServiceClient>("BrandService",
//     o => { o.Address = new Uri(AppConstants.BrandAndProductServiceUrl); });
// builder.Services.AddGrpcClient<ProductService.ProductServiceClient>("ProductService",
//     o => { o.Address = new Uri(AppConstants.BrandAndProductServiceUrl); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
SwaggerConfiguration.Configure(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FairWear Gateway API");
    options.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();