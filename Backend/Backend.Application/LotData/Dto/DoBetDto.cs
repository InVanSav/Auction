namespace Backend.Application.LotData.Dto;

/// <summary>
/// Сделать ставку
/// </summary>
public class DoBetDto
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
    /// Уникальный идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
}