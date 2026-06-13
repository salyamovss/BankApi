using BankApi.dal.DTOs.Transaction;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController(TransactionService transactionService) : ControllerBase
{
    [HttpPost("transfer")]
    [SwaggerOperation(Summary = "Перевод между счетами", Description = "Переводит средства между счетами. Поддерживает конвертацию валют.")]
    [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferResponse>> Transfer(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromBody] TransferRequest request)
    {
        return Ok(await transactionService.Transfer(userId, request));
    }

    [HttpGet("account/{accountId:int}")]
    [SwaggerOperation(Summary = "История транзакций по счету")]
    [ProducesResponseType(typeof(List<TransferResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TransferResponse>>> GetByAccountId(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromRoute] int accountId)
    {
        return Ok(await transactionService.GetByAccountId(accountId, userId));
    }
}