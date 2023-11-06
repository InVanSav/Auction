using Backend.Application.IRepositories;
using Backend.Domain.Enum;
using Backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Notifications;

/// <summary>
/// Обработчик уведомлений
/// </summary>
public class NotificationHandler : INotificationHandler
{
    /// <summary>
    /// Контекст хаба для уведомлений
    /// </summary>
    private readonly IHubContext<AuctionHub> _hubContext;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="hubContext">Контекст хаба для уведомлений</param>
    public NotificationHandler(IHubContext<AuctionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    /// Уведомление о сделанной ставке
    /// </summary>
    public async Task MadeBetNoticeAsync(string lotName, string username)
    {
        await _hubContext.Clients.All.SendAsync("MadeBet", lotName, username);
    }

    /// <summary>
    /// Уведомление о созданном лоте
    /// </summary>
    public async Task CreatedLotNoticeAsync(string auctionName, string lotName)
    {
        await _hubContext.Clients.All.SendAsync("CreatedLot", auctionName, lotName);
    }

    /// <summary>
    /// Уведомление о смене статуса аукциона
    /// </summary>
    public async Task ChangedAuctionStatusNoticeAsync(string auctionName, State state)
    {
        await _hubContext.Clients.All.SendAsync("ChangedAuctionStatus", auctionName, state);
    }

    /// <summary>
    /// Уведомление о смене статуса лота
    /// </summary>
    public async Task ChangedLotStatusNoticeAsync(string auctionName, string lotName, State state)
    {
        await _hubContext.Clients.All.SendAsync("ChangedLotStatus", auctionName, lotName, state);
    }

    /// <summary>
    /// Уведомление о продаже лота 
    /// </summary>
    public async Task SoldLotNoticeAsync(string auctionName, string lotName, decimal buyoutPrice)
    {
        await _hubContext.Clients.All.SendAsync("SoldLot", auctionName, lotName, buyoutPrice);
    }
}