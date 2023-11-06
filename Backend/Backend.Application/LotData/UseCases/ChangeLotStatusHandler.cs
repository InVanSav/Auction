using Backend.Application.AuctionData.IRepository;
using Backend.Application.IRepositories;
using Backend.Application.LotData.Dto;
using Backend.Application.LotData.IRepository;

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
    /// Репозиторий пользователя
    /// </summary>
    private readonly ILotRepository _lotRepository;

    /// <summary>
    /// Обработчик уведомлений
    /// </summary>
    private readonly INotificationHandler _notificationHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    /// <param name="notificationHandler">Обработчик уведомлений</param>
    /// <param name="lotRepository">Репозиторий лота</param>
    public ChangeLotStatusHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler,
        ILotRepository lotRepository)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
        _lotRepository = lotRepository;
    }

    /// <summary>
    /// Сменить статус лота
    /// </summary>
    /// <param name="changeLotStatusDto">Изменить статус лота</param>
    public async Task ChangeLotStatusAsync(ChangeLotStatusDto changeLotStatusDto)
    {
        var auction = await _auctionRepository.SelectAsync(changeLotStatusDto.AuctionId);
        if (auction is null) return;

        var result = auction.ChangeLotStatus(changeLotStatusDto.LotId, changeLotStatusDto.State);
        if (result.IsFailed) return;

        await _auctionRepository.UpdateAsync(auction);

        var lot = await _lotRepository.SelectAsync(changeLotStatusDto.LotId);
        if (lot is null) return;

        await _notificationHandler.ChangedLotStatusNoticeAsync(auction.Name!, lot.Name, changeLotStatusDto.State);
    }
}