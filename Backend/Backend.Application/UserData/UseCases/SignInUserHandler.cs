using Backend.Application.UserData.Dto;
using Backend.Application.UserData.IRepository;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Backend.Application.UserData.UseCases;

/// <summary>
/// Авторизация пользователя
/// </summary>
public class SignInUserHandler
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
    /// Доступ к контексту запроса
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователя</param>
    /// <param name="authorizationHandler">Обработчик авторизации</param>
    /// <param name="httpContextAccessor">Доступ к контексту запроса</param>
    public SignInUserHandler(IUserRepository userRepository, IOptions<AuthorizationHandler> authorizationHandler,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorizationHandler = authorizationHandler.Value;
    }

    /// <summary>
    /// Авторизовать пользователя
    /// </summary>
    /// <param name="userSignInDto">Аутентификация пользователя</param>
    public async Task<Result<UserDto>> SignInUserAsync(UserSignInDto userSignInDto)
    {
        var user = await _userRepository.SelectByNameAsync(userSignInDto.Email);

        if (!_authorizationHandler.VerifyUserData(userSignInDto.Email, userSignInDto.Password, user))
            return Result.Fail<UserDto>("Пользователя не существует или в данных ошибка");

        var token = _authorizationHandler.CreateToken(userSignInDto.Email);

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(".AspNet.Application.Id", token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60),
                HttpOnly = true
            });

        return Result.Ok(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}