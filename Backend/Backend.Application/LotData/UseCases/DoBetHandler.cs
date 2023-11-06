using Backend.Application.AuctionData.IRepository;
using Backend.Application.IRepositories;
using Backend.Application.LotData.Dto;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Установка ставки
/// </summary>
public class DoBetHandler
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
    public DoBetHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
    }

    /// <summary>
    /// Сделать ставку
    /// </summary>
    /// <param name="doBetDto">Сделать ставку</param>
    public async Task DoBetAsync(DoBetDto doBetDto)
    {
        var auction = await _auctionRepository.SelectAsync(doBetDto.AuctionId);
        if (auction is null) return;

        var result = auction.DoBet(doBetDto.LotId, doBetDto.UserId);

        if (result.IsFailed) return;

        await _auctionRepository.UpdateAsync(auction);
        await _notificationHandler.MadeBetNoticeAsync();
    }
}