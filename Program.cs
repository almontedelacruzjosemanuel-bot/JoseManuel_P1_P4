using Parcial1_P4_JoseManuel.Services;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<NumbersService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<NumbersService>();
    await service.InitializeDatabase();
}

app.Run();