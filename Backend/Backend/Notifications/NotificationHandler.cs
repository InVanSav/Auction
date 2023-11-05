using Backend.Application;
using Backend.Application.Repositories;
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
    public async Task MadeBetNoticeAsync()
    {
        await _hubContext.Clients.All.SendAsync("MadeBet");
    }

    /// <summary>
    /// Уведомление о созданном лоте
    /// </summary>
    public async Task CreatedLotNoticeAsync()
    {
        await _hubContext.Clients.All.SendAsync("CreatedLot");
    }

    /// <summary>
    /// Уведомление о смене статуса аукциона
    /// </summary>
    public async Task ChangedAuctionStatusNoticeAsync()
    {
        await _hubContext.Clients.All.SendAsync("ChangedAuctionStatus");
    }

    /// <summary>
    /// Уведомление о смене статуса лота
    /// </summary>
    public async Task ChangedLotStatusNoticeAsync()
    {
        await _hubContext.Clients.All.SendAsync("ChangedLotStatus");
    }

    /// <summary>
    /// Уведомление о продаже лота 
    /// </summary>
    public async Task SoldLotNoticeAsync()
    {
        await _hubContext.Clients.All.SendAsync("SoldLot");
    }
}