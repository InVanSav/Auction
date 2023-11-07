using Backend.Application.LotData.Dto;
using Backend.Application.LotData.IRepository;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Обработчик получения списка лотов
/// </summary>
public class GetLotsHandler
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
    public GetLotsHandler(ILotRepository lotRepository, FileHandler.FileHandler fileHandler)
    {
        _lotRepository = lotRepository;
        _fileHandler = fileHandler;
    }

    /// <summary>
    /// Получение списка лотов
    /// </summary>
    /// <returns>список лотов</returns>
    public async Task<IReadOnlyCollection<LotDto>> GetLots(Guid userId)
    {
        var lotsDto = new List<LotDto>();
        var lots = await _lotRepository.SelectManyAsync();
        var filteredLots = lots.Where(lot => lot.Bets.Any(bet => bet.UserId == userId));

        foreach (var lot in filteredLots)
        {
            var imagesData = await _fileHandler.GetImagesFromHostAsync(lot.Name);

            lotsDto.Add(new LotDto()
            {
                Id = lot.Id,
                Name = lot.Name,
                Description = lot.Description,
                StartPrice = lot.StartPrice,
                BuyoutPrice = lot.BuyoutPrice,
                BetStep = lot.BetStep,
                State = lot.State,
                Bets = lot.Bets,
                Images = imagesData
            });
        }

        return lotsDto;
    }
}