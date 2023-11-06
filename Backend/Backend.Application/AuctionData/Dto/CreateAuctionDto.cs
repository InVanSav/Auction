using Newtonsoft.Json;

namespace Backend.Application.AuctionData.Dto;

/// <summary>
/// Класс для создания аукциона
/// </summary>
public record CreateAuctionDto
{
    /// <summary>
    /// Название аукциона
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Описание аукциона
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя-создателя
    /// </summary>
    [JsonProperty("authorId")]
    public Guid AuthorId { get; init; }
}