using Backend.Domain.Enum;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs;

/// <summary>
/// Хаб аукциона для отправки уведомлений
/// </summary>
public class AuctionHub : Hub
{
    /// <summary>
    /// Уведомление о ставке
    /// </summary>
    [HubMethodName("MadeBet")]
    public async Task SendMadeBetNoticeAsync(string lotName, string username)
        => await Clients.All.SendAsync("MadeBet", lotName, username);

    /// <summary>
    /// Уведомление о создании лота
    /// </summary>
    [HubMethodName("CreatedLot")]
    public async Task SendCreatedLotNoticeAsync(string auctionName, string lotName)
        => await Clients.All.SendAsync("CreatedLot", auctionName, lotName);

    /// <summary>
    /// Уведомление о смене статуса аукциона
    /// </summary>
    [HubMethodName("ChangedAuctionStatus")]
    public async Task SendChangedAuctionStatusNoticeAsync(string auctionName, State state)
        => await Clients.All.SendAsync("ChangedAuctionStatus", auctionName, state);

    /// <summary>
    /// Уведомление о смене статуса лота
    /// </summary>
    [HubMethodName("ChangedLotStatus")]
    public async Task SendChangedLotStatusNoticeAsync(string auctionName, string lotName, State state)
        => await Clients.All.SendAsync("ChangedLotStatus", auctionName, lotName, state);

    /// <summary>
    /// Уведомление о продаже лота
    /// </summary>
    [HubMethodName("SoldLot")]
    public async Task SendSoldLotNoticeAsync(string auctionName, string lotName, decimal buyoutPrice)
        => await Clients.All.SendAsync("SoldLot", auctionName, lotName, buyoutPrice);
}