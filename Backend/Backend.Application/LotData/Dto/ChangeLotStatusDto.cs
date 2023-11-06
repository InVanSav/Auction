using Backend.Domain.Enum;
using Newtonsoft.Json;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Изменение статуса лота
/// </summary>
public record ChangeLotStatusDto
{
    /// <summary>
    /// Уникальный идентификатор аукциона
    /// </summary>
    [JsonProperty("auctionId")]
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Уникальный идентификатор лота
    /// </summary>
    [JsonProperty("lotId")]
    public Guid LotId { get; init; }

    /// <summary>
    /// Новое состояние
    /// </summary>
    [JsonProperty("state")]
    public State State { get; init; }
}