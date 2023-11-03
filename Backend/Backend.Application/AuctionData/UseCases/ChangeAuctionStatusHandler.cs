using Backend.Application.AuctionData.Dto;
using Backend.Application.AuctionData.IRepository;
using Backend.Domain.Enum;

namespace Backend.Application.AuctionData.UseCases;

/// <summary>
/// Смена статуса аукциона
/// </summary>
public class ChangeAuctionStatusHandler
{
    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// Обработчик уведомлений
    /// </summary>
    private readonly INotificationHandler _notificationHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    /// <param name="notificationHandler"></param>
    public ChangeAuctionStatusHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
    }

    /// <summary>
    /// Изменить статус аукциона
    /// </summary>
    /// <param name="newStatus">ID лота и новый статус</param>
    public async Task ChangeAuctionStatus(ChangeStatusDto newStatus)
    {
        var auction = await _auctionRepository.SelectAsync(newStatus.AuctionId);

        auction.ChangeStatus(newStatus.State);
        foreach (var lot in auction.Lots.Values)
        {
            if (lot.State is State.Completed or State.Canceled) continue;
            lot.ChangeStatus(newStatus.State);
        }

        await _auctionRepository.UpdateAsync(auction);

        await _notificationHandler.ChangedAuctionStatusNoticeAsync();
    }
}