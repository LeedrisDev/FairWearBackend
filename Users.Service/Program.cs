using Users.Service.Config;
using Users.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Validate required environment variables
EnvironmentValidator.ValidateRequiredVariables();

// Add services to the container.
builder.Services.AddGrpc();

DependencyInjectionConfiguration.Configure(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserService>();
app.MapGrpcService<UserProductHistoryService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();