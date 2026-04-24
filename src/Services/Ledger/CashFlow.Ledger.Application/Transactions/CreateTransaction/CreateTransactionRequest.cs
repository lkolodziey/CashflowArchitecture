namespace CashFlow.Ledger.Application.Transactions.CreateTransaction;

public sealed record CreateTransactionRequest(
    string Type,
    decimal Amount,
    DateOnly TransactionDate,
    string? Description);

public sealed record CreateTransactionResponse(
    Guid Id,
    string Type,
    decimal Amount,
    DateOnly TransactionDate,
    string? Description,
    DateTimeOffset CreatedAtUtc);