using Newtonsoft.Json;

namespace Backend.Application.UserData.Dto;

/// <summary>
/// Регистрация пользователя
/// </summary>
public record UserSignInDto
{
    /// <summary>
    /// Почта пользователя
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    [JsonProperty("password")]
    public string Password { get; init; } = string.Empty;
}