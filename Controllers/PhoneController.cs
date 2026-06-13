using BankApi.dal.DTOs.Phone;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

/// <summary>
/// Управление телефонными номерами пользователя
/// </summary>
[ApiController]
[Route("api/phones")]
public class PhoneController(PhoneService phoneService) : ControllerBase
{
    /// <summary>
    /// Добавить телефонный номер пользователю
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="request">Данные нового телефона</param>
    /// <returns>Добавленный телефон</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Добавить телефон пользователю")]
    [ProducesResponseType(typeof(PhoneResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<PhoneResponse>> Add(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromBody] AddPhoneRequest request)
    {
        var phone = await phoneService.Add(userId, request);
        return Created(string.Empty, phone);
    }

    /// <summary>
    /// Удалить телефонный номер пользователя
    /// </summary>
    /// <param name="userId">ID текущего пользователя</param>
    /// <param name="id">ID телефона для удаления</param>
    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Удалить телефон пользователя")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromHeader(Name = "X-User-Id")] int userId,
        [FromRoute] int id)
    {
        await phoneService.Delete(id, userId);
        return NoContent();
    }
}