using Backend.Application.AuctionData.IRepository;
using Backend.Application.IRepositories;
using Backend.Application.LotData.IRepository;
using Backend.Domain.Entity;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.LotData.UseCases;

/// <summary>
/// Создание лота
/// </summary>
public class CreateLotHandler
{
    /// <summary>
    /// Репозиторий лота
    /// </summary>
    private readonly ILotRepository _lotRepository;

    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// Обработчик уведомлений
    /// </summary>
    private readonly INotificationHandler _notificationHandler;

    /// <summary>
    /// Обработчик файлов
    /// </summary>
    private readonly FileHandler.FileHandler _fileHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="lotRepository">Репозиторий лота</param>
    /// <param name="notificationHandler">Обработчик уведомлений</param>
    /// <param name="fileHandler">Обработчик файлов</param>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    public CreateLotHandler(ILotRepository lotRepository, INotificationHandler notificationHandler,
        FileHandler.FileHandler fileHandler, IAuctionRepository auctionRepository)
    {
        _lotRepository = lotRepository;
        _notificationHandler = notificationHandler;
        _fileHandler = fileHandler;
        _auctionRepository = auctionRepository;
    }

    /// <summary>
    /// Создать лот
    /// </summary>
    /// <param name="formCollection">Изображения лота</param>
    public async Task CreateLotAsync(IFormCollection formCollection)
    {
        if (!decimal.TryParse(formCollection["startPrice"][0], out var startPrice) ||
            !decimal.TryParse(formCollection["betStep"][0], out var betStep) ||
            !Guid.TryParse(formCollection["auctionId"][0], out var auctionId)) return;

        var lot = new Lot(
            Guid.NewGuid(),
            formCollection["name"],
            formCollection["description"],
            auctionId,
            startPrice,
            0,
            betStep,
            State.Awaiting);

        var images = await _fileHandler.SaveImagesToHostAsync(
            formCollection.Files.GetFiles("images"), lot.Id, lot.Name);

        if (images == null) return;

        lot.SetImages(images);
        await _lotRepository.CreateAsync(lot);

        var auction = await _auctionRepository.SelectAsync(auctionId);
        if (auction is null) return;

        await _notificationHandler.CreatedLotNoticeAsync(auction.Name!, lot.Name);
    }
}