using BrandAndProductDatabase.API.Config;

var builder = WebApplication.CreateBuilder(args);

// Validate required environment variables
EnvironmentValidator.ValidateRequiredVariables();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

SwaggerConfiguration.Configure(builder.Services);
DependencyInjectionConfiguration.Configure(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();