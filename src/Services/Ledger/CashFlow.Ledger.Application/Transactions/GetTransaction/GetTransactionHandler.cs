using CashFlow.Ledger.Application.Abstractions;
namespace CashFlow.Ledger.Application.Transactions.GetTransaction;
public sealed record TransactionDetailsResponse(Guid Id, string Type, decimal Amount, DateOnly TransactionDate, string? Description, DateTimeOffset CreatedAtUtc);
public sealed class GetTransactionHandler(ITransactionRepository repository)
{
    public async Task<TransactionDetailsResponse?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var t = await repository.GetByIdAsync(id, cancellationToken);
        return t is null ? null : new TransactionDetailsResponse(t.Id, t.Type.ToString(), t.Amount, t.TransactionDate, t.Description, t.CreatedAtUtc);
    }
}
