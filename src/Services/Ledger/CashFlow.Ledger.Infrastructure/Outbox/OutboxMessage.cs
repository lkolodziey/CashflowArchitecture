namespace CashFlow.Ledger.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public Guid Id { get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; private set; }
    public DateTimeOffset? ProcessedAtUtc { get; private set; }
    public string Status { get; private set; } = "Pending";
    public int RetryCount { get; private set; }

    public static OutboxMessage Create(string eventType, string payload, DateTimeOffset occurredAtUtc) => new()
        { Id = Guid.NewGuid(), EventType = eventType, Payload = payload, OccurredAtUtc = occurredAtUtc };

    public void MarkAsProcessed()
    {
        Status = "Processed";
        ProcessedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = "Failed";
        RetryCount++;
    }
}