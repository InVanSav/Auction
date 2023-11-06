namespace Backend.Application.LotData.Dto;

/// <summary>
/// Выкупить лот
/// </summary>
public record BuyoutDto
{
    /// <summary>
    /// Уникальный идентификатор аукциона
    /// </summary>
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Уникальный идентификатор лота
    /// </summary>
    public Guid LotId { get; init; }
}