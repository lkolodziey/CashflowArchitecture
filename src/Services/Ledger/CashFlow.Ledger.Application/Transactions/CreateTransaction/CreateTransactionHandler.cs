using CashFlow.Ledger.Application.Abstractions;
using CashFlow.Ledger.Domain.Transactions;
using CashFlow.SharedContracts;

namespace CashFlow.Ledger.Application.Transactions.CreateTransaction;

public sealed class CreateTransactionHandler(
    ITransactionRepository repository,
    IOutboxWriter outboxWriter,
    IUnitOfWork unitOfWork)
{
    public async Task<CreateTransactionResponse> HandleAsync(CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<TransactionType>(request.Type, true, out var type))
            throw new ArgumentException("Type must be credit or debit.", nameof(request.Type));

        var transaction = Transaction.Create(type, request.Amount, request.TransactionDate, request.Description);
        await repository.AddAsync(transaction, cancellationToken);

        outboxWriter.Add(
            new TransactionCreatedEvent(Guid.NewGuid(), transaction.Id, transaction.Type.ToString(), transaction.Amount,
                transaction.TransactionDate, transaction.Description, transaction.CreatedAtUtc), "transaction.created");
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateTransactionResponse(transaction.Id, transaction.Type.ToString(), transaction.Amount,
            transaction.TransactionDate, transaction.Description, transaction.CreatedAtUtc);
    }
}