using CashFlow.DailyConsolidation.Application.Abstractions;
using CashFlow.DailyConsolidation.Domain.DailyBalances;
using CashFlow.SharedContracts;

namespace CashFlow.DailyConsolidation.Application.DailyBalances;

public sealed class ApplyTransactionCreatedHandler(IDailyBalanceRepository balances, IProcessedMessageRepository processed, IConsolidationUnitOfWork unitOfWork)
{
    public async Task HandleAsync(TransactionCreatedEvent evt, CancellationToken cancellationToken)
    {
        if (await processed.WasProcessedAsync(evt.MessageId, cancellationToken)) return;

        var balance = await balances.GetAsync(evt.TransactionDate, cancellationToken);
        if (balance is null)
        {
            balance = new DailyBalance(evt.TransactionDate);
            await balances.AddAsync(balance, cancellationToken);
        }

        if (evt.Type.Equals("Credit", StringComparison.OrdinalIgnoreCase))
            balance.ApplyCredit(evt.Amount);
        else
            balance.ApplyDebit(evt.Amount);

        await processed.MarkProcessedAsync(evt.MessageId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
