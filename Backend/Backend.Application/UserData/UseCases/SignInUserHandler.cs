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
    private readonly AuthorizationHandler _authorityHandler;

    /// <summary>
    /// Доступ к контексту запроса
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователя</param>
    /// <param name="authorityHandler">Обработчик авторизации</param>
    /// <param name="httpContextAccessor">Доступ к контексту запроса</param>
    public SignInUserHandler(IUserRepository userRepository, IOptions<AuthorizationHandler> authorityHandler,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorityHandler = authorityHandler.Value;
    }

    /// <summary>
    /// Авторизовать пользователя
    /// </summary>
    /// <param name="userSignInDto">Аутентификация пользователя</param>
    public async Task<Result<UserDto>> SignInUserAsync(UserSignInDto userSignInDto)
    {
        var user = await _userRepository.SelectByNameAsync(userSignInDto.Email);

        if (!_authorityHandler.VerifyUserData(userSignInDto.Email, userSignInDto.Password, user))
            return Result.Fail<UserDto>("Пользователя не существует или в данных ошибка");

        var token = _authorityHandler.CreateToken(userSignInDto.Email);

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(".AspNet.Application.Id", token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60),
                HttpOnly = true,
                Secure = true
            });

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}