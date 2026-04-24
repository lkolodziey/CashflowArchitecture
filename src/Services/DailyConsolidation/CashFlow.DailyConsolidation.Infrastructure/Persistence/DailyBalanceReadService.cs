using System.Data;
using CashFlow.DailyConsolidation.Application.Abstractions;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace CashFlow.DailyConsolidation.Infrastructure.Persistence;

public sealed class DailyBalanceReadService(IConfiguration configuration) : IDailyBalanceReadService
{
    public async Task<DailyBalanceDto?> GetAsync(DateOnly date, CancellationToken cancellationToken)
    {
        const string sql = """
                           select
                               balance_date::text as Date,
                               total_credits as TotalCredits,
                               total_debits as TotalDebits,
                               (total_credits - total_debits) as ClosingBalance
                           from consolidation.daily_balances
                           where balance_date = @Date::date
                           limit 1
                           """;
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("Postgres"));
        return await connection.QuerySingleOrDefaultAsync<DailyBalanceDto>(
            new CommandDefinition(
                sql,
                new { Date = date.ToDateTime(TimeOnly.MinValue) },
                cancellationToken: cancellationToken));
    }
}