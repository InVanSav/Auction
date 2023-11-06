using Newtonsoft.Json;

namespace Backend.Application.UserData.Dto;

/// <summary>
/// Пользователь
/// </summary>
public record UserDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

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