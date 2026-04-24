using CashFlow.DailyConsolidation.Domain.DailyBalances;

namespace CashFlow.DailyConsolidation.Application.Abstractions;

public interface IDailyBalanceRepository
{
    Task<DailyBalance?> GetAsync(DateOnly date, CancellationToken cancellationToken);
    Task AddAsync(DailyBalance balance, CancellationToken cancellationToken);
}

public interface IProcessedMessageRepository
{
    Task<bool> WasProcessedAsync(Guid messageId, CancellationToken cancellationToken);
    Task MarkProcessedAsync(Guid messageId, CancellationToken cancellationToken);
}

public interface IConsolidationUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IDailyBalanceReadService
{
    Task<DailyBalanceDto?> GetAsync(DateOnly date, CancellationToken cancellationToken);
}

public sealed record DailyBalanceDto(
    string Date,
    decimal TotalCredits,
    decimal TotalDebits,
    decimal ClosingBalance);