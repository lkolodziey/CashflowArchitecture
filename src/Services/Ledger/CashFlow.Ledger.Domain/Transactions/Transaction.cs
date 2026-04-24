using CashFlow.SharedKernel;

namespace CashFlow.Ledger.Domain.Transactions;

public sealed class Transaction : Entity<Guid>
{
    private Transaction()
    {
    }

    private Transaction(Guid id, TransactionType type, decimal amount, DateOnly transactionDate, string? description,
        DateTimeOffset createdAtUtc)
    {
        Id = id;
        Type = type;
        Amount = amount;
        TransactionDate = transactionDate;
        Description = description;
        CreatedAtUtc = createdAtUtc;
    }

    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public DateOnly TransactionDate { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    public static Transaction Create(TransactionType type, decimal amount, DateOnly transactionDate,
        string? description)
    {
        if (amount <= 0) 
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        
        if (description?.Length > 250)
            throw new ArgumentException("Description must have at most 250 characters.", nameof(description));
        
        return new Transaction(Guid.NewGuid(), type, decimal.Round(amount, 2), transactionDate, description,
            DateTimeOffset.UtcNow);
    }
}