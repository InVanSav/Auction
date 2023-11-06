using Backend.Application.AuctionData.IRepository;

namespace Backend.Application.AuctionData.UseCases;

/// <summary>
/// Установка даты окончания аукциона
/// </summary>
public class SetDateEndAuctionHandler
{
    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    public SetDateEndAuctionHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    /// <summary>
    /// Установить дату окончания аукциона
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    public async Task SetDateEndAuctionAsync(Guid id)
    {
        var auction = await _auctionRepository.SelectAsync(id);
        if (auction is null) return;

        auction.SetDateEnd();

        await _auctionRepository.UpdateAsync(auction);
    }
}