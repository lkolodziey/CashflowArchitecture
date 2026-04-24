using CashFlow.Ledger.Domain.Transactions;
using FluentAssertions;
using Xunit;

namespace CashFlow.Ledger.Tests;

public sealed class TransactionTests
{
    [Fact]
    public void Create_ShouldRejectNonPositiveAmount() => FluentActions
        .Invoking(() => Transaction.Create(TransactionType.Credit, 0, DateOnly.FromDateTime(DateTime.UtcNow), null))
        .Should().Throw<ArgumentOutOfRangeException>();

    [Fact]
    public void Create_ShouldRoundAmountToTwoDecimals()
    {
        var t = Transaction.Create(TransactionType.Debit, 10.999m, new DateOnly(2026, 1, 1), "test");
        t.Amount.Should().Be(11.00m);
    }
}