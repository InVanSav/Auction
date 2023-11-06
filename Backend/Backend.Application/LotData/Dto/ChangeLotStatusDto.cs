using Backend.Domain.Enum;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Изменение статуса лота
/// </summary>
public record ChangeLotStatusDto
{
    /// <summary>
    /// Уникальный идентификатор аукциона
    /// </summary>
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Уникальный идентификатор лота
    /// </summary>
    public Guid LotId { get; init; }

    /// <summary>
    /// Новое состояние
    /// </summary>
    public State State { get; init; }
}