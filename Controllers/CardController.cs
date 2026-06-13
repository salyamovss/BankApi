using BankApi.dal.DTOs.Card;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

/// <summary>
/// Управление банковскими картами
/// </summary>
[ApiController]
[Route("api/cards")]
public class CardController(CardService cardService) : ControllerBase
{
    /// <summary>
    /// Выпустить новую карту для счёта
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="request">Данные для выпуска карты</param>
    /// <returns>Выпущенная карта</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Выпустить новую карту")]
    [ProducesResponseType(typeof(CardResponseMessage), StatusCodes.Status201Created)]
    public async Task<ActionResult<CardResponseMessage>> Create(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromBody] CreateCardRequest request)
    {
        var card = await cardService.Create(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = card.Card.Id }, card);
    }

    /// <summary>
    /// Заблокировать карту
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="id">ID карты</param>
    /// <returns>Обновлённая карта со статусом Blocked</returns>
    [HttpDelete("{id:int}/block")]
    [SwaggerOperation(Summary = "Заблокировать карту")]
    [ProducesResponseType(typeof(CardResponseMessage), StatusCodes.Status200OK)]
    public async Task<ActionResult<CardResponseMessage>> Block(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromRoute] int id)
    {
        return Ok(await cardService.Block(id, userId));
    }

    /// <summary>
    /// Разблокировать карту
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="id">ID карты</param>
    /// <returns>Обновлённая карта со статусом Active</returns>
    [HttpPut("{id:int}/unblock")]
    [SwaggerOperation(Summary = "Разблокировать карту")]
    [ProducesResponseType(typeof(CardResponseMessage), StatusCodes.Status200OK)]
    public async Task<ActionResult<CardResponseMessage>> Unblock(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromRoute] int id)
    {
        return Ok(await cardService.Unblock(id, userId));
    }

    /// <summary>
    /// Перевыпустить карту
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="id">ID карты для перевыпуска</param>
    /// <returns>Новая карта с новым номером</returns>
    [HttpPut("{id:int}/reissue")]
    [SwaggerOperation(Summary = "Перевыпустить карту")]
    [ProducesResponseType(typeof(CardResponseMessage), StatusCodes.Status200OK)]
    public async Task<ActionResult<CardResponseMessage>> Reissue(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromRoute] int id)
    {
        return Ok(await cardService.Reissue(id, userId));
    }

    /// <summary>
    /// Получить карту по ID
    /// </summary>
    /// <param name="id">ID карты</param>
    /// <returns>Информация о карте</returns>
    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Получить карту по ID")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CardResponse>> GetById([FromRoute] int id) =>
        Ok(await cardService.GetById(id));

    /// <summary>
    /// Получить все карты по ID счёта
    /// </summary>
    /// <param name="accountId">ID счёта</param>
    /// <returns>Список карт счёта</returns>
    [HttpGet("account/{accountId:int}")]
    [SwaggerOperation(Summary = "Получить все карты счета")]
    [ProducesResponseType(typeof(List<CardResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CardResponse>>> GetByAccountId([FromRoute] int accountId) =>
        Ok(await cardService.GetByAccountId(accountId));
}