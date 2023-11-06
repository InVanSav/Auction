using Backend.Application.AuctionData.IRepository;
using Backend.Application.LotData.Dto;
using Backend.Domain.Entity;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Обновление лота
/// </summary>
public class UpdateLotHandler
{
    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    public UpdateLotHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    /// <summary>
    /// Обновить лот
    /// </summary>
    /// <param name="entity">Модель лота</param>
    public async Task UpdateLotAsync(LotDto entity)
    {
        var auction = await _auctionRepository.SelectAsync(entity.AuctionId);
        if (auction is null) return;

        var lot = new Lot(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.AuctionId,
            entity.StartPrice,
            entity.BuyoutPrice,
            entity.BetStep,
            entity.State);

        var images = lot.Images.Select(image => new Image
        {
            Id = image.Id,
            LotId = image.LotId,
            Path = image.Path
        });

        var result = auction.UpdateLot(lot.Id, lot.Name, lot.Description, lot.BetStep, images);
        if (result.IsFailed) return;

        await _auctionRepository.UpdateAsync(auction);
    }
}