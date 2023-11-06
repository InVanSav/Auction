using Newtonsoft.Json;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Выкупить лот
/// </summary>
public record BuyoutDto
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
}