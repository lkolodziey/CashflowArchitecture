using CashFlow.DailyConsolidation.Application.Abstractions;
using CashFlow.DailyConsolidation.Application.DailyBalances;
using CashFlow.DailyConsolidation.Infrastructure.Messaging;
using CashFlow.DailyConsolidation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CashFlow.DailyConsolidation.Infrastructure.DependencyInjection;
public static class ConsolidationInfrastructureExtensions
{
    public static IServiceCollection AddConsolidationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ConsolidationDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Postgres")));
        services.AddScoped<IDailyBalanceRepository, DailyBalanceRepository>(); 
        services.AddScoped<IProcessedMessageRepository, ProcessedMessageRepository>();
        services.AddScoped<IConsolidationUnitOfWork>(sp => sp.GetRequiredService<ConsolidationDbContext>()); 
        services.AddScoped<IDailyBalanceReadService, DailyBalanceReadService>();
        services.AddScoped<ApplyTransactionCreatedHandler>(); 
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq")); 
        services.AddHostedService<ConsolidationConsumerBackgroundService>();
        return services;
    }
}
