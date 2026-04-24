using CashFlow.DailyConsolidation.Infrastructure.DependencyInjection;
using CashFlow.SharedObservability;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());
builder.Services.AddControllers(); builder.Services.AddOpenApi(); builder.Services.AddConsolidationInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Postgres")!);
var app = builder.Build(); app.UseMiddleware<CorrelationIdMiddleware>(); app.MapOpenApi(); app.MapControllers(); app.MapHealthChecks("/health"); app.Run();
