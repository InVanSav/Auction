using Backend.Application.LotData.Dto;
using Backend.Application.LotData.IRepository;
using Backend.Domain.Entity;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Обработчик получения списка лотов по аукциону
/// </summary>
public class GetLotsByAuctionHandler
{
    /// <summary>
    /// Репозиторий лота
    /// </summary>
    private readonly ILotRepository _lotRepository;

    /// <summary>
    /// Обработчик изображений
    /// </summary>
    private readonly FileHandler.FileHandler _fileHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="lotRepository">Репозиторий лота</param>
    /// <param name="fileHandler">Обработчик изображений</param>
    public GetLotsByAuctionHandler(ILotRepository lotRepository, FileHandler.FileHandler fileHandler)
    {
        _lotRepository = lotRepository;
        _fileHandler = fileHandler;
    }

    /// <summary>
    /// Получение списка лотов
    /// </summary>
    /// <returns>список лотов</returns>
    public async Task<IReadOnlyCollection<LotDto>> GetLotsByAuction(Guid auctionId)
    {
        var lotsDto = new List<LotDto>();
        var lots = await _lotRepository.SelectManyByAuctionAsync(auctionId);

        foreach (var lot in lots)
        {
            var imagesData = await _fileHandler.GetImagesFromHostAsync(lot.Name);

            var betsDto = new List<Bet>();

            foreach (var bet in lot.Bets)
            {
                betsDto.Add(new Bet
                {
                    Id = bet.Id,
                    Value = bet.Value,
                    LotId = bet.LotId,
                    UserId = bet.UserId,
                    DateTime = bet.DateTime
                });
            }

            lotsDto.Add(new LotDto()
            {
                Id = lot.Id,
                Name = lot.Name,
                Description = lot.Description,
                StartPrice = lot.StartPrice,
                BuyoutPrice = lot.BuyoutPrice,
                BetStep = lot.BetStep,
                State = lot.State,
                Bets = betsDto,
                Images = imagesData
            });
        }

        return lotsDto;
    }
}