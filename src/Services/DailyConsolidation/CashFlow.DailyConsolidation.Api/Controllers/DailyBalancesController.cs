using CashFlow.DailyConsolidation.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
namespace CashFlow.DailyConsolidation.Api.Controllers;
[ApiController]
[Route("api/daily-balances")]
public sealed class DailyBalancesController(IDailyBalanceReadService readService) : ControllerBase
{
    [HttpGet("{date}")]
    public async Task<ActionResult<DailyBalanceDto>> Get(DateOnly date, CancellationToken cancellationToken)
    {
        var result = await readService.GetAsync(date, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
