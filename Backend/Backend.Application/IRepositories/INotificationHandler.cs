namespace Backend.Application.Repositories;

/// <summary>
/// Интерфейс уведомлений
/// </summary>
public interface INotificationHandler
{
    /// <summary>
    /// Уведомление о сделанной ставке
    /// </summary>
    public Task MadeBetNoticeAsync();

    /// <summary>
    /// Уведомление о созданном лоте
    /// </summary>
    public Task CreatedLotNoticeAsync();

    /// <summary>
    /// Уведомление о смене статуса аукциона
    /// </summary>
    public Task ChangedAuctionStatusNoticeAsync();

    /// <summary>
    /// Уведомление о смене статуса лота
    /// </summary>
    public Task ChangedLotStatusNoticeAsync();

    /// <summary>
    /// Уведомление о продаже лота
    /// </summary>
    public Task SoldLotNoticeAsync();
}