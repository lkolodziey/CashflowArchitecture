using CashFlow.SharedKernel;
namespace CashFlow.DailyConsolidation.Domain.DailyBalances;
public sealed class DailyBalance : Entity<DateOnly>
{
    private DailyBalance() { }
    public DailyBalance(DateOnly date) { Id = date; BalanceDate = date; }
    public DateOnly BalanceDate { get; private set; }
    public decimal TotalCredits { get; private set; }
    public decimal TotalDebits { get; private set; }
    public decimal ClosingBalance => TotalCredits - TotalDebits;
    public DateTimeOffset UpdatedAtUtc { get; private set; }
    public void ApplyCredit(decimal amount) { TotalCredits += amount; UpdatedAtUtc = DateTimeOffset.UtcNow; }
    public void ApplyDebit(decimal amount) { TotalDebits += amount; UpdatedAtUtc = DateTimeOffset.UtcNow; }
}
