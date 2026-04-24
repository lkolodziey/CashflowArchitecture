using CashFlow.DailyConsolidation.Application.Abstractions;
using CashFlow.DailyConsolidation.Domain.DailyBalances;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.DailyConsolidation.Infrastructure.Persistence;

public sealed class ConsolidationDbContext(DbContextOptions<ConsolidationDbContext> options)
    : DbContext(options), IConsolidationUnitOfWork
{
    public DbSet<DailyBalance> DailyBalances => Set<DailyBalance>();
    public DbSet<ProcessedMessage> ProcessedMessages => Set<ProcessedMessage>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("consolidation");
        modelBuilder.Entity<ProcessedMessage>(builder =>
        {
            builder.ToTable("processed_messages", "consolidation");

            builder.HasKey(x => x.MessageId);

            builder.Property(x => x.MessageId)
                .HasColumnName("message_id");

            builder.Property(x => x.ProcessedAtUtc)
                .HasColumnName("processed_at_utc")
                .IsRequired();
        });

        modelBuilder.Entity<DailyBalance>(builder =>
        {
            builder.ToTable("daily_balances", "consolidation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.BalanceDate)
                .HasColumnName("balance_date")
                .IsRequired();

            builder.Property(x => x.TotalCredits)
                .HasColumnName("total_credits")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.TotalDebits)
                .HasColumnName("total_debits")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Ignore(x => x.ClosingBalance);

            builder.Property(x => x.UpdatedAtUtc)
                .HasColumnName("updated_at_utc")
                .IsRequired();
        });
    }
}