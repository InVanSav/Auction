using Backend.Domain.Entity;
using Backend.Domain.Enum;
using Newtonsoft.Json;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Лот
/// </summary>
public record LotDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Название лота
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Описание лота
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Уникальный идентификатор аукциона лота
    /// </summary>
    [JsonProperty("auctionId")]
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Стартовая цена лота
    /// </summary>
    [JsonProperty("startPrice")]
    public decimal StartPrice { get; init; }

    /// <summary>
    /// Цена выкупа лота
    /// </summary>
    [JsonProperty("buyoutPrice")]
    public decimal BuyoutPrice { get; init; }

    /// <summary>
    /// Шаг ставки лота
    /// </summary>
    [JsonProperty("betStep")]
    public decimal BetStep { get; init; }

    /// <summary>
    /// Статус лота
    /// </summary>
    [JsonProperty("state")]
    public State State { get; init; }

    /// <summary>
    /// Ставки лота
    /// </summary>
    public IEnumerable<Bet> Bets { get; init; } = new List<Bet>();

    /// <summary>
    /// Изображения лота
    /// </summary>
    public IEnumerable<object> Images { get; init; } = new List<object>();
}