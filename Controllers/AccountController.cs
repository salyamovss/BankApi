using BankApi.dal.DTOs.Account;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

/// <summary>
/// Управление банковскими счетами
/// </summary>
[ApiController]
[Route("api/accounts")]
public class AccountController(AccountService accountService) : ControllerBase
{
    /// <summary>
    /// Создать новый банковский счёт
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="request">Данные для создания счёта</param>
    /// <returns>Созданный счёт</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Создание счета")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AccountResponse>> Create(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromBody] CreateAccountRequest request)
    {
        var account = await accountService.Create(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }

    /// <summary>
    /// Получить счёт по ID
    /// </summary>
    /// <param name="id">ID счёта</param>
    /// <returns>Информация о счёте</returns>
    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Получить информацию о счете по ID")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AccountResponse>> GetById([FromRoute] int id) =>
        Ok(await accountService.GetById(id));

    /// <summary>
    /// Получить все счета текущего пользователя
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <returns>Список счетов пользователя</returns>
    [HttpGet("user")]
    [SwaggerOperation(Summary = "Получить все счета текущего пользователя")]
    [ProducesResponseType(typeof(List<AccountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AccountResponse>>> GetByUserId(
        [FromHeader(Name = "X-User-Id")] int userId) =>
        Ok(await accountService.GetByUserId(userId));

    /// <summary>
    /// Закрыть банковский счёт
    /// </summary>
    /// <param name="id">ID счёта для закрытия</param>
    /// <param name="request">Опциональный ID счёта для перевода остатка</param>
    /// <returns>Результат закрытия счёта</returns>
    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Закрыть банковский счет",
        Description = "Если на счету есть деньги, необходимо в Query параметрах передать targetAccountId для автоматического перевода и конвертации остатка средств на другой активный счет пользователя."
    )]
    [ProducesResponseType(typeof(CloseAccountResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CloseAccountResponse>> Close(
        [FromRoute] int id,
        [FromQuery] CloseAccountRequest request)
    {
        return Ok(await accountService.Close(id, request));
    }

    /// <summary>
    /// Восстановить закрытый счёт
    /// </summary>
    /// <param name="id">ID счёта для восстановления</param>
    /// <returns>Восстановленный счёт</returns>
    [HttpPost("{id:int}/restore")]
    [SwaggerOperation(
        Summary = "Восстановить закрытый счёт",
        Description = "Переводит статус ранее закрытого счёта обратно в Active и сбрасывает дату закрытия."
    )]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AccountResponse>> Restore([FromRoute] int id)
    {
        var restoredAccount = await accountService.Restore(id);
        return Ok(restoredAccount);
    }
}