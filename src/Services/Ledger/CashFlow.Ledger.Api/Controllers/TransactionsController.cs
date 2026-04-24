using CashFlow.Ledger.Application.Transactions.CreateTransaction;
using CashFlow.Ledger.Application.Transactions.GetTransaction;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Ledger.Api.Controllers;
[ApiController]
[Route("api/transactions")]
public sealed class TransactionsController(CreateTransactionHandler createHandler, GetTransactionHandler getHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateTransactionResponse>> Create(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var response = await createHandler.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDetailsResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await getHandler.HandleAsync(id, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }
}
