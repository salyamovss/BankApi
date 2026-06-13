using BankApi.dal.DTOs.User;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankApi.Controllers;

/// <summary>
/// Управление пользователями
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : ControllerBase
{
    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    /// <param name="request">Данные нового пользователя</param>
    /// <returns>Созданный пользователь</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Регистрация нового пользователя")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request)
    {
        var user = await userService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Получить профиль пользователя по ID
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns>Профиль пользователя</returns>
    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Получить профиль пользователя по ID")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] int id)
    {
        var user = await userService.GetById(id);
        return Ok(user);
    }

    /// <summary>
    /// Восстановить деактивированного пользователя
    /// </summary>
    /// <param name="request">Email деактивированного пользователя</param>
    /// <returns>Восстановленный пользователь</returns>
    [HttpPost("restore")]
    [SwaggerOperation(Summary = "Восстановить деактивированного пользователя", Description = "Переводит флаг IsActive обратно в true для ранее удаленного пользователя")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Restore([FromBody] RestoreUserRequest request)
    {
        return Ok(await userService.Restore(request));
    }

    /// <summary>
    /// Обновить личные данные пользователя
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <param name="request">Новые данные пользователя</param>
    /// <returns>Обновлённый пользователь</returns>
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Обновить личные данные пользователя", Description = "Позволяет изменить ФИО, Email и пол. Паспорт и телефоны изменяются через отдельные эндпоинты.")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
    {
        var user = await userService.UpdateAsync(id, request);
        return Ok(user);
    }

    /// <summary>
    /// Получить список пользователей с фильтрацией и пагинацией
    /// </summary>
    /// <param name="filter">Параметры фильтрации и пагинации</param>
    /// <returns>Отфильтрованный список пользователей</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Получить список клиентов", Description = "Поддерживает фильтрацию по имени, фамилии, email, статусу и пагинацию.")]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetFiltered([FromQuery] UserFilterRequest filter)
    {
        return Ok(await userService.GetFilteredAsync(filter));
    }

    /// <summary>
    /// Деактивировать пользователя
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns>Результат деактивации со списком закрытых счетов</returns>
    [HttpPatch("{id:int}/deactivate")]
    [SwaggerOperation(Summary = "Деактивировать пользователя", Description = "Блокирует все карты и закрывает счета с нулевым балансом.")]
    [ProducesResponseType(typeof(DeactivateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DeactivateResponse>> Deactivate([FromRoute] int id)
    {
        return Ok(await userService.Deactivate(id));
    }
}