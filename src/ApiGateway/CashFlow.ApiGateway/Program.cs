using CashFlow.ApiGateway;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddHealthChecks();
var app = builder.Build();
app.UseMiddleware<ApiKeyMiddleware>();
app.MapReverseProxy();
app.MapHealthChecks("/health");
app.Run();