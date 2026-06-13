using BankApi.dal.DTOs.User;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Регистрация нового пользователя")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request)
    {
        var user = await userService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Получить профиль пользователя по ID")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] int id)
    {
        var user = await userService.GetById(id);
        return Ok(user);
    }
    
    [HttpPost("restore")]
    [SwaggerOperation(Summary = "Восстановить деактивированного пользователя", Description = "Переводит флаг IsActive обратно в true для ранее удаленного пользователя")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Restore([FromBody] RestoreUserRequest request)
    {
        return Ok(await userService.Restore(request));
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Обновить личные данные пользователя", Description = "Позволяет изменить ФИО, Email и пол. Паспорт и телефоны изменяются через отдельные эндпоинты.")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
    {
        var user = await userService.UpdateAsync(id, request);
        return Ok(user);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получить список клиентов", Description = "Поддерживает фильтрацию по имени, фамилии, email, статусу и пагинацию.")]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetFiltered([FromQuery] UserFilterRequest filter)
    {
        return Ok(await userService.GetFilteredAsync(filter));
    }
    
    [HttpPatch("{id:int}/deactivate")]
    [SwaggerOperation(Summary = "Деактивировать пользователя", Description = "Блокирует все карты и закрывает счета с нулевым балансом.")]
    [ProducesResponseType(typeof(DeactivateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DeactivateResponse>> Deactivate([FromRoute] int id)
    {
        return Ok(await userService.Deactivate(id));
    }
    
}