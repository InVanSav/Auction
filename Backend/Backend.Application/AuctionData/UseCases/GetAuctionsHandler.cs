using Backend.Application.AuctionData.Dto;
using Backend.Application.AuctionData.IRepository;
using Backend.Application.LotData.Dto;
using FluentResults;

namespace Backend.Application.AuctionData.UseCases;

/// <summary>
/// Получение списка аукционов
/// </summary>
public class GetAuctionsHandler
{
    /// <summary>
    /// Репозиторий аукциона
    /// </summary>
    private readonly IAuctionRepository _auctionRepository;

    /// <summary>
    /// Обработчик файлов
    /// </summary>
    private readonly FileHandler.FileHandler _fileHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="auctionRepository">Репозиторий аукциона</param>
    /// <param name="fileHandler">Обработчик файлов</param>
    public GetAuctionsHandler(IAuctionRepository auctionRepository, FileHandler.FileHandler fileHandler)
    {
        _auctionRepository = auctionRepository;
        _fileHandler = fileHandler;
    }

    /// <summary>
    /// Получить аукцион
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    /// <returns>Модель аукциона</returns>
    public async Task<Result<AuctionDto>> GetAuctionByIdAsync(Guid id)
    {
        var auction = await _auctionRepository.SelectAsync(id);
        if (auction is null) return Result.Fail<AuctionDto>("Не существует такого аукциона");

        var lotsDto = new List<LotDto>();
        var lots = auction.Lots;

        foreach (var lot in lots.Values)
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

        return Result.Ok(new AuctionDto
        {
            Id = auction.Id,
            Name = auction.Name,
            Description = auction.Description,
            DateStart = auction.DateStart,
            DateEnd = auction.DateEnd,
            AuthorId = auction.AuthorId,
            State = auction.State,
            Lots = lotsDto
        });
    }

    /// <summary>
    /// Получить список аукционов
    /// </summary>
    /// <returns>Список моделей аукциона</returns>
    public async Task<Result<IReadOnlyCollection<AuctionDto>>> GetAuctions()
    {
        var auctionsDto = new List<AuctionDto>();
        var auctions = await _auctionRepository.SelectManyAsync();

        foreach (var auction in auctions)
        {
            var lotsDto = new List<LotDto>();
            var lots = auction.Lots;

            foreach (var lot in lots.Values)
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

            auctionsDto.Add(
                new AuctionDto
                {
                    Id = auction.Id,
                    Name = auction.Name,
                    Description = auction.Description,
                    DateStart = auction.DateStart,
                    DateEnd = auction.DateEnd,
                    AuthorId = auction.AuthorId,
                    State = auction.State,
                    Lots = lotsDto
                });
        }

        return Result.Ok<IReadOnlyCollection<AuctionDto>>(auctionsDto);
    }
}