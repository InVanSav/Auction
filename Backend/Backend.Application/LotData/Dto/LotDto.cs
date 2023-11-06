using Backend.Domain.Entity;
using Backend.Domain.Enum;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Лот
/// </summary>
public record LotDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название лота
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Описание лота
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Уникальный идентификатор аукциона лота
    /// </summary>
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Стартовая цена лота
    /// </summary>
    public decimal StartPrice { get; init; }

    /// <summary>
    /// Цена выкупа лота
    /// </summary>
    public decimal BuyoutPrice { get; init; }

    /// <summary>
    /// Шаг ставки лота
    /// </summary>
    public decimal BetStep { get; init; }

    /// <summary>
    /// Статус лота
    /// </summary>
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