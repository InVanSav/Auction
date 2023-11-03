using Backend.Application.UserData.Dto;
using Backend.Application.UserData.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер пользователя
/// </summary>
[Authorize]
[ApiController]
[Route("api/user/")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Обработчик удаления пользователя
    /// </summary>
    private readonly DeleteUserHandler _deleteHandler;

    /// <summary>
    /// Обработчик получения пользователя
    /// </summary>
    private readonly GetUserByIdHandler _getByIdHandler;

    /// <summary>
    /// Обработчик получения списка пользователей
    /// </summary>
    private readonly GetUsersHandler _getHandler;

    /// <summary>
    /// Обработчик аутентификации пользователя
    /// </summary>
    private readonly SignInUserHandler _signInHandler;

    /// <summary>
    /// Обработчик регистрации пользователя
    /// </summary>
    private readonly SignUpUserHandler _signUpHandler;

    /// <summary>
    /// Обработчик обновления пользователя
    /// </summary>
    private readonly UpdateUserHandler _updateHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="deleteHandler">Обработчик удаления пользователя</param>
    /// <param name="getByIdHandler">Обработчик получения пользователя</param>
    /// <param name="getHandler">Обработчик получения списка пользователей</param>
    /// <param name="signInHandler">Обработчик аутентификации пользователя</param>
    /// <param name="signUpHandler">Обработчик регистрации пользователя</param>
    /// <param name="updateHandler">Обработчик обновления пользователя</param>
    public UserController(DeleteUserHandler deleteHandler, GetUserByIdHandler getByIdHandler,
        GetUsersHandler getHandler, SignInUserHandler signInHandler, SignUpUserHandler signUpHandler,
        UpdateUserHandler updateHandler)
    {
        _deleteHandler = deleteHandler;
        _getByIdHandler = getByIdHandler;
        _getHandler = getHandler;
        _signInHandler = signInHandler;
        _signUpHandler = signUpHandler;
        _updateHandler = updateHandler;
    }

    /// <summary>
    /// Запрос на удаление пользователя
    /// </summary>
    /// <param name="id">Уникальный индентификатор пользователя</param>
    [HttpDelete("delete/{id:guid}")]
    public async Task DeleteUserAsync(Guid id)
    {
        await _deleteHandler.DeleteUserAsync(id);
    }

    /// <summary>
    /// Запрос на получение пользователя по уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный индентификатор пользователя</param>
    /// <returns>Пользователь</returns>
    [HttpGet("get-by-id/{id:guid}")]
    public Task<UserDto> GetUserByIdAsync(Guid id)
    {
        return _getByIdHandler.GetUserById(id);
    }

    /// <summary>
    /// Запрос на получение списка пользователей
    /// </summary>
    /// <returns>Список пользователей</returns>
    [HttpGet("get-list")]
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        return await _getHandler.GetUsersAsync();
    }

    /// <summary>
    /// Запрос на аутентификацию пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <returns>Пользователь</returns>
    [AllowAnonymous]
    [HttpPost("sign-in")]
    public Task<UserDto> SignInUserAsync([FromBody] UserSignInDto user)
    {
        return _signInHandler.SignInUserAsync(user.Email, user.Password);
    }

    /// <summary>
    /// Запрос на регистрацию пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task SignUpUserAsync([FromBody] UserDto user)
    {
        await _signUpHandler.SignUpUserAsync(user);
    }

    /// <summary>
    /// Запрос на обновление пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    [HttpPut("update")]
    public async Task UpdateUserAsync([FromBody] UserDto user)
    {
        await _updateHandler.UpdateUserAsync(user);
    }
}