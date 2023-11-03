using Microsoft.AspNetCore.Http;

namespace Backend.Application.UserData.UseCases;

/// <summary>
/// Разлогинивание пользователя
/// </summary>
public class SignOutUserHandler
{
    /// <summary>
    /// Доступ к контексту запроса
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="httpContextAccessor">Доступ к контексту запроса</param>
    public SignOutUserHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    /// <summary>
    /// Разлогинивание пользователя
    /// </summary>
    public void SignOutUserAsync()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(".AspNet.Application.Id");
    }
}