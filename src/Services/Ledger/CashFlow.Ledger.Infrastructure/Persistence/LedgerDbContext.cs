using System.Text.Json;
using CashFlow.Ledger.Application.Abstractions;
using CashFlow.Ledger.Domain.Transactions;
using CashFlow.Ledger.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Ledger.Infrastructure.Persistence;

public sealed class LedgerDbContext(DbContextOptions<LedgerDbContext> options)
    : DbContext(options), IUnitOfWork, IOutboxWriter
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public void Add<T>(T message, string eventType)
    {
        OutboxMessages.Add(OutboxMessage.Create(eventType, JsonSerializer.Serialize(message, message!.GetType()),
            DateTimeOffset.UtcNow));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ledger");
        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.ToTable("transactions", "ledger");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.TransactionDate)
                .HasColumnName("transaction_date")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .IsRequired();
        });
        
        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.ToTable("outbox_messages", "integration");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.EventType)
                .HasColumnName("event_type")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Payload)
                .HasColumnName("payload")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(x => x.OccurredAtUtc)
                .HasColumnName("occurred_at_utc")
                .IsRequired();

            builder.Property(x => x.ProcessedAtUtc)
                .HasColumnName("processed_at_utc");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.RetryCount)
                .HasColumnName("retry_count")
                .IsRequired();
        });
    }
}