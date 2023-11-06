namespace Backend.Application.UserData.Dto;

/// <summary>
/// Регистрация пользователя
/// </summary>
public record UserSignInDto
{
    /// <summary>
    /// Почта пользователя
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; init; } = string.Empty;
}