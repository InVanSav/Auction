using Backend.Domain.Enum;

namespace Backend.Application.LotData.Dto;

/// <summary>
/// Изменение статуса лота
/// </summary>
public class ChangeLotStatusDto
{
    /// <summary>
    /// Уникальный идентификатор аукциона
    /// </summary>
    public Guid AuctionId { get; set; }

    /// <summary>
    /// Уникальный идентификатор лота
    /// </summary>
    public Guid LotId { get; set; }

    /// <summary>
    /// Новое состояние
    /// </summary>
    public State State { get; set; }
}