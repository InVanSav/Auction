using Backend.Application.UserData.Dto;
using Backend.Application.UserData.IRepository;
using Backend.Domain.Entity;
using Microsoft.Extensions.Options;

namespace Backend.Application.UserData.UseCases;

/// <summary>
/// Регистрация пользователя
/// </summary>
public class SignUpUserHandler
{
    /// <summary>
    /// Репозиторий пользователя
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Обработчик авторизации
    /// </summary>
    private readonly AuthorizationHandler _authorizationHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователя</param>
    /// <param name="authorizationHandler">Обработчик авторизации</param>
    public SignUpUserHandler(IUserRepository userRepository, IOptions<AuthorizationHandler> authorizationHandler)
    {
        _userRepository = userRepository;
        _authorizationHandler = authorizationHandler.Value;
    }

    /// <summary>
    /// Зарегистрировать пользователя
    /// </summary>
    /// <param name="entity">Пользователь</param>
    public async Task SignUpUserAsync(UserDto entity)
    {
        var password = _authorizationHandler.HashAndSaltPassword(entity.Password);

        await _userRepository.CreateAsync(new User(
            Guid.NewGuid(),
            entity.Name,
            entity.Email,
            password));
    }
}