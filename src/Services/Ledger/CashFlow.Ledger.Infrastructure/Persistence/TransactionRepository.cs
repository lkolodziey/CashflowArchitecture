using CashFlow.Ledger.Application.Abstractions;
using CashFlow.Ledger.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Ledger.Infrastructure.Persistence;

public sealed class TransactionRepository(LedgerDbContext db) : ITransactionRepository
{
    public Task AddAsync(Transaction transaction, CancellationToken cancellationToken) =>
        db.Transactions.AddAsync(transaction, cancellationToken).AsTask();

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}