using CashFlow.DailyConsolidation.Application.Abstractions;
using CashFlow.DailyConsolidation.Domain.DailyBalances;
using Microsoft.EntityFrameworkCore;
namespace CashFlow.DailyConsolidation.Infrastructure.Persistence;
public sealed class DailyBalanceRepository(ConsolidationDbContext db) : IDailyBalanceRepository
{
    public Task<DailyBalance?> GetAsync(DateOnly date, CancellationToken cancellationToken) => db.DailyBalances.FirstOrDefaultAsync(x => x.Id == date, cancellationToken);
    public Task AddAsync(DailyBalance balance, CancellationToken cancellationToken) => db.DailyBalances.AddAsync(balance, cancellationToken).AsTask();
}
public sealed class ProcessedMessageRepository(ConsolidationDbContext db) : IProcessedMessageRepository
{
    public Task<bool> WasProcessedAsync(Guid messageId, CancellationToken cancellationToken) => db.ProcessedMessages.AnyAsync(x => x.MessageId == messageId, cancellationToken);
    public Task MarkProcessedAsync(Guid messageId, CancellationToken cancellationToken) => db.ProcessedMessages.AddAsync(new ProcessedMessage{MessageId=messageId,ProcessedAtUtc=DateTimeOffset.UtcNow}, cancellationToken).AsTask();
}
