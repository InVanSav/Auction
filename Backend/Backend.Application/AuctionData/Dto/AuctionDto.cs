using Backend.Application.LotData.Dto;
using Backend.Domain.Enum;
using Newtonsoft.Json;

namespace Backend.Application.AuctionData.Dto;

/// <summary>
/// Аукцион
/// </summary>
public record AuctionDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

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
    /// Начало аукциона
    /// </summary>
    [JsonProperty("dateStart")]
    public DateTime? DateStart { get; init; }

    /// <summary>
    /// Завершение аукциона
    /// </summary>
    [JsonProperty("dateEnd")]
    public DateTime? DateEnd { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя-создателя
    /// </summary>
    [JsonProperty("authorId")]
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Статус ставки
    /// </summary>
    [JsonProperty("state")]
    public State State { get; init; }

    /// <summary>
    /// Лоты аукциона
    /// </summary>
    public IEnumerable<LotDto> Lots { get; init; } = new List<LotDto>();
}