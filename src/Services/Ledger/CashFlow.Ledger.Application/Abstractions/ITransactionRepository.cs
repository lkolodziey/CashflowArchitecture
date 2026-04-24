using CashFlow.Ledger.Domain.Transactions;
namespace CashFlow.Ledger.Application.Abstractions;
public interface ITransactionRepository { Task AddAsync(Transaction transaction, CancellationToken cancellationToken); Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken); }
