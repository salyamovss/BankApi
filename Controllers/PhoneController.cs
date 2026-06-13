using BankApi.dal.DTOs.Phone;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

[ApiController]
[Route("api/phones")]
public class PhoneController(PhoneService phoneService) : ControllerBase
{
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