namespace CashFlow.Ledger.Application.Abstractions;
public interface IUnitOfWork { Task<int> SaveChangesAsync(CancellationToken cancellationToken); }
