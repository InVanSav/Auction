using Backend.Application.AuctionData.IRepository;
using Backend.Application.LotData.Dto;
using Backend.Application.Repositories;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Выкуп лота
/// </summary>
public class BuyoutLotHandler
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
    public BuyoutLotHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
    }

    /// <summary>
    /// Выкупить лот
    /// </summary>
    /// <param name="buyoutDto">Выкупить лот</param>
    public async Task BuyoutLotAsync(BuyoutDto buyoutDto)
    {
        var auction = await _auctionRepository.SelectAsync(buyoutDto.AuctionId);

        var result = auction.BuyoutLot(buyoutDto.LotId);
        if (result.IsFailed) return;

        await _auctionRepository.UpdateAsync(auction);

        await _notificationHandler.SoldLotNoticeAsync();
    }
}