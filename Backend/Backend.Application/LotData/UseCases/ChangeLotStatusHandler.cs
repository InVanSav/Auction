using Backend.Application.AuctionData.IRepository;
using Backend.Application.LotData.Dto;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Смена статуса лота
/// </summary>
public class ChangeLotStatusHandler
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
    /// <param name="notificationHandler">Обработчик уведомлений</param>
    public ChangeLotStatusHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
    }

    /// <summary>
    /// Сменить статус лота
    /// </summary>
    /// <param name="changeLotStatusDto">Изменить статус лота</param>
    public async Task ChangeLotStatusAsync(ChangeLotStatusDto changeLotStatusDto)
    {
        var auction = await _auctionRepository.SelectAsync(changeLotStatusDto.AuctionId);

        auction.ChangeLotStatus(changeLotStatusDto.LotId, changeLotStatusDto.State);

        await _auctionRepository.UpdateAsync(auction);

        await _notificationHandler.ChangedLotStatusNoticeAsync();
    }
}