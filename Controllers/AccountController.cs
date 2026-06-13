using BankApi.dal.DTOs.Account;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(AccountService accountService) : ControllerBase
{
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

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Получить информацию о счете по ID")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AccountResponse>> GetById([FromRoute] int id) => 
        Ok(await accountService.GetById(id));

    [HttpGet("user")]
    [SwaggerOperation(Summary = "Получить все счета текущего пользователя")]
    [ProducesResponseType(typeof(List<AccountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AccountResponse>>> GetByUserId([FromHeader(Name = "X-User-Id")] int userId) => 
        Ok(await accountService.GetByUserId(userId));
    
    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Закрыть банковский счет", 
        Description = "Если на счету есть деньги, необходимо в Query параметрах передать targetAccountId для автоматического перевода и конвертации остатка средств на другой активный счет пользователя."
    )]
    [ProducesResponseType(typeof(CloseAccountResponse), StatusCodes.Status200OK)] 
    public async Task<ActionResult<CloseAccountResponse>> Close([FromRoute] int id, [FromQuery] CloseAccountRequest request)
    {
        return Ok(await accountService.Close(id, request));
    }
    
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