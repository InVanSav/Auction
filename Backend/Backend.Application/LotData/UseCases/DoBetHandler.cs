using Backend.Application.AuctionData.IRepository;
using Backend.Application.IRepositories;
using Backend.Application.LotData.Dto;
using Backend.Application.LotData.IRepository;
using Backend.Application.UserData.IRepository;

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
    /// Репозиторий пользователя
    /// </summary>
    private readonly IUserRepository _userRepository;

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
    /// <param name="userRepository">Репозиторий пользователя</param>
    /// <param name="lotRepository">Репозиторий лота</param>
    public DoBetHandler(IAuctionRepository auctionRepository, INotificationHandler notificationHandler,
        IUserRepository userRepository, ILotRepository lotRepository)
    {
        _auctionRepository = auctionRepository;
        _notificationHandler = notificationHandler;
        _userRepository = userRepository;
        _lotRepository = lotRepository;
    }

    /// <summary>
    /// Сделать ставку
    /// </summary>
    /// <param name="doBetDto">Сделать ставку</param>
    public async Task DoBetAsync(DoBetDto doBetDto)
    {
        var auction = await _auctionRepository.SelectAsync(doBetDto.AuctionId);
        if (auction is null) return;

        var user = await _userRepository.SelectAsync(doBetDto.UserId);
        if (user is null) return;

        var lot = await _lotRepository.SelectAsync(doBetDto.LotId);
        if (lot is null) return;

        var result = auction.DoBet(doBetDto.LotId, doBetDto.UserId);
        if (result.IsFailed) return;

        await _auctionRepository.UpdateAsync(auction);
        await _notificationHandler.MadeBetNoticeAsync(lot.Name, user.Name);
    }
}