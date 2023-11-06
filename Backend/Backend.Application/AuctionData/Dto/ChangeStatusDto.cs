using Backend.Domain.Enum;
using Newtonsoft.Json;

namespace Backend.Application.AuctionData.Dto;

/// <summary>
/// Класс для смены статуса аукциона
/// </summary>
public record ChangeStatusDto
{
    /// <summary>
    /// Уникальный идентификтор аукциона
    /// </summary>
    [JsonProperty("auctionId")]
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Новый статус
    /// </summary>
    [JsonProperty("state")]
    public State State { get; init; }
}