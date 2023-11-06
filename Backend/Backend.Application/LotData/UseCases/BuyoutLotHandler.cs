using Backend.Application.AuctionData.IRepository;
using Backend.Application.IRepositories;
using Backend.Application.LotData.Dto;
using Backend.Application.LotData.IRepository;

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
    /// Репозиторий лота
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
    public BuyoutLotHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler,
        ILotRepository lotRepository)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
        _lotRepository = lotRepository;
    }

    /// <summary>
    /// Выкупить лот
    /// </summary>
    /// <param name="buyoutDto">Выкупить лот</param>
    public async Task BuyoutLotAsync(BuyoutDto buyoutDto)
    {
        var auction = await _auctionRepository.SelectAsync(buyoutDto.AuctionId);
        if (auction is null) return;

        var result = auction.BuyoutLot(buyoutDto.LotId);
        if (result.IsFailed) return;

        await _auctionRepository.UpdateAsync(auction);

        var lot = await _lotRepository.SelectAsync(buyoutDto.LotId);
        if (lot is null) return;

        await _notificationHandler.SoldLotNoticeAsync(auction.Name!, lot.Name, lot.BuyoutPrice);
    }
}