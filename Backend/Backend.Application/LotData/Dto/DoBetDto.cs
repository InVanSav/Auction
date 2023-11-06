using Newtonsoft.Json;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Сделать ставку
/// </summary>
public record DoBetDto
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
    /// Уникальный идентификатор пользователя
    /// </summary>
    [JsonProperty("userId")]
    public Guid UserId { get; init; }
}