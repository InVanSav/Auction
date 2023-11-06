using Backend.Domain.Enum;

namespace Backend.Application.IRepositories;

/// <summary>
/// Интерфейс уведомлений
/// </summary>
public interface INotificationHandler
{
    /// <summary>
    /// Уведомление о сделанной ставке
    /// </summary>
    public Task MadeBetNoticeAsync(string lotName, string username);

    /// <summary>
    /// Уведомление о созданном лоте
    /// </summary>
    public Task CreatedLotNoticeAsync(string auctionName, string lotName);

    /// <summary>
    /// Уведомление о смене статуса аукциона
    /// </summary>
    public Task ChangedAuctionStatusNoticeAsync(string auctionName, State state);

    /// <summary>
    /// Уведомление о смене статуса лота
    /// </summary>
    public Task ChangedLotStatusNoticeAsync(string auctionName, string lotName, State state);

    /// <summary>
    /// Уведомление о продаже лота
    /// </summary>
    public Task SoldLotNoticeAsync(string auctionName, string lotName, decimal buyoutPrice);
}