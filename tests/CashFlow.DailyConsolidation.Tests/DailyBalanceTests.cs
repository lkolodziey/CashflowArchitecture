using CashFlow.DailyConsolidation.Domain.DailyBalances;
using FluentAssertions;
using Xunit;

namespace CashFlow.DailyConsolidation.Tests;

public sealed class DailyBalanceTests
{
    [Fact]
    public void ClosingBalance_ShouldBeCreditsMinusDebits()
    {
        var b = new DailyBalance(new DateOnly(2026, 1, 1));
        b.ApplyCredit(100);
        b.ApplyDebit(40);
        b.ClosingBalance.Should().Be(60);
    }
}