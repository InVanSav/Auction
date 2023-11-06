using Backend.Application.AuctionData.IRepository;

namespace Backend.Application.AuctionData.UseCases;

/// <summary>
/// Установка даты начала аукциона
/// </summary>
public class SetDateStartAuctionHandler
{
    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    public SetDateStartAuctionHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    /// <summary>
    /// Установить дату начала аукциона
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    public async Task SetDateStartAuctionAsync(Guid id)
    {
        var auction = await _auctionRepository.SelectAsync(id);
        if (auction is null) return;

        auction.SetDateStart();

        await _auctionRepository.UpdateAsync(auction);
    }
}