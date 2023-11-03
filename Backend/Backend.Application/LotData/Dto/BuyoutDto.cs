namespace Backend.Application.LotData.Dto;

/// <summary>
/// Выкупить лот
/// </summary>
public class BuyoutDto
{
    /// <summary>
    /// Уникальный идентификатор аукциона
    /// </summary>
    public Guid AuctionId { get; set; }

    /// <summary>
    /// Уникальный идентификатор лота
    /// </summary>
    public Guid LotId { get; set; }
}