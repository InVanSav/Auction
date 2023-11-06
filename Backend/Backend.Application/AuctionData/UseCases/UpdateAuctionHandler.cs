using Backend.Application.AuctionData.Dto;
using Backend.Application.AuctionData.IRepository;

namespace Backend.Application.AuctionData.UseCases;

/// <summary>
/// Обновление аукциона
/// </summary>
public class UpdateAuctionHandler
{
    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    public UpdateAuctionHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    /// <summary>
    /// Обновить аукцион
    /// </summary>
    /// <param name="entity">Модель аукциона</param>
    public async Task UpdateAuctionAsync(AuctionDto entity)
    {
        var auction = await _auctionRepository.SelectAsync(entity.Id);
        if (auction is null) return;

        auction.UpdateInformation(entity.Name, entity.Description);

        await _auctionRepository.UpdateAsync(auction);
    }
}