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
    /// .ctor
    /// </summary>
    /// <param name="lotRepository">Репозиторий лота</param>
    public GetLotsHandler(ILotRepository lotRepository)
    {
        _lotRepository = lotRepository;
    }

    /// <summary>
    /// Получение списка лотов
    /// </summary>
    /// <returns>список лотов</returns>
    public async Task<IReadOnlyCollection<LotDto>> GetLots()
    {
        var lotsDto = new List<LotDto>();
        var lots = await _lotRepository.SelectManyAsync();

        // Пока проект тестовый, принято решение оставить упрощенную версию сохранения изображений
        foreach (var lot in lots)
        {
            var imagesData = new List<object>();

            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Backend.Images", lot.Name);
            if (!Directory.Exists(imagesPath)) continue;

            var imageFiles = Directory.GetFiles(imagesPath);

            foreach (var imageFile in imageFiles)
            {
                var imageBytes = await File.ReadAllBytesAsync(imageFile);
                var base64Image = Convert.ToBase64String(imageBytes);
                var imageName = Path.GetFileName(imageFile);

                var imageData = new { name = imageName, data = base64Image };
                imagesData.Add(imageData);
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
                Bets = lot.Bets,
                Images = imagesData
            });
        }

        return lotsDto;
    }
}