using CashFlow.Ledger.Application.Abstractions;
using CashFlow.Ledger.Application.Transactions.CreateTransaction;
using CashFlow.Ledger.Application.Transactions.GetTransaction;
using CashFlow.Ledger.Infrastructure.Outbox;
using CashFlow.Ledger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Ledger.Infrastructure.DependencyInjection;

public static class LedgerInfrastructureExtensions
{
    public static IServiceCollection AddLedgerInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LedgerDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Postgres")));
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LedgerDbContext>());
        services.AddScoped<IOutboxWriter>(sp => sp.GetRequiredService<LedgerDbContext>());
        services.AddScoped<CreateTransactionHandler>();
        services.AddScoped<GetTransactionHandler>();
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.AddHostedService<OutboxPublisherBackgroundService>();
        return services;
    }
}