using BankApi.dal.DTOs.Transaction;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

/// <summary>
/// Управление транзакциями и переводами между счетами
/// </summary>
[ApiController]
[Route("api/transactions")]
public class TransactionController(TransactionService transactionService) : ControllerBase
{
    /// <summary>
    /// Перевести средства между счетами
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="request">Данные перевода: счёт отправителя, счёт получателя, сумма</param>
    /// <returns>Результат перевода с информацией о конвертации</returns>
    [HttpPost("transfer")]
    [SwaggerOperation(Summary = "Перевод между счетами", Description = "Переводит средства между счетами. Поддерживает конвертацию валют.")]
    [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferResponse>> Transfer(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromBody] TransferRequest request)
    {
        return Ok(await transactionService.Transfer(userId, request));
    }

    /// <summary>
    /// Получить историю транзакций по счёту
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="accountId">ID счёта</param>
    /// <returns>Список транзакций по счёту в порядке убывания даты</returns>
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