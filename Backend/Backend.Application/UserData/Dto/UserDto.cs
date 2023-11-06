namespace Backend.Application.UserData.Dto;

/// <summary>
/// Пользователь
/// </summary>
public record UserDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Почта пользователя
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; init; } = string.Empty;
}