namespace CashFlow.SharedContracts;

public sealed record TransactionCreatedEvent(
    Guid MessageId,
    Guid TransactionId,
    string Type,
    decimal Amount,
    DateOnly TransactionDate,
    string? Description,
    DateTimeOffset OccurredAtUtc);
