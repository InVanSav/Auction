namespace Backend.Application.LotData.Dto;

/// <summary>
/// Сделать ставку
/// </summary>
public record DoBetDto
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
    /// Уникальный идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }
}