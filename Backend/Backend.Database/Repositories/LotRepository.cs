using System.Data;
using Backend.Application.FileHandler;
using Backend.Application.LotData.IRepository;
using Backend.Database.PostgreSQL;
using Backend.Domain.Entity;
using Backend.Domain.Enum;

namespace Backend.Database.Repositories;

/// <summary>
/// Репозиторий Лота
/// </summary>
public class LotRepository : ILotRepository
{
    /// <summary>
    /// Обработчик запросов к базе данных
    /// </summary>
    private readonly PgsqlHandler _pgsqlHandler;

    /// <summary>
    /// Обработчик файлов
    /// </summary>
    private readonly FileHandler _fileHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="pgsqlHandler">Обработчик запросов к базе данных</param>
    /// <param name="fileHandler">Обработчик файлов</param>
    public LotRepository(PgsqlHandler pgsqlHandler, FileHandler fileHandler)
    {
        _pgsqlHandler = pgsqlHandler;
        _fileHandler = fileHandler;
    }

    /// <summary>
    /// Запрос на добавление Лота
    /// </summary>
    /// <param name="entity">Лот</param>
    /// <returns>True или False</returns>
    public async Task CreateAsync(Lot entity)
    {
        await _pgsqlHandler.ExecuteAsync("Lot.InsertLot",
            new KeyValuePair<string, object>("id", entity.Id),
            new KeyValuePair<string, object>("name", entity.Name),
            new KeyValuePair<string, object>("description", entity.Description),
            new KeyValuePair<string, object>("auctionId", entity.AuctionId),
            new KeyValuePair<string, object>("startPrice", entity.StartPrice),
            new KeyValuePair<string, object>("betStep", entity.BetStep));

        foreach (var image in entity.Images)
        {
            await _pgsqlHandler.ExecuteAsync("Image.InsertImage",
                new KeyValuePair<string, object>("id", image.Id),
                new KeyValuePair<string, object>("lotId", image.LotId),
                new KeyValuePair<string, object>("path", image.Path!));
        }
    }

    /// <summary>
    /// Запрос на получение Лота
    /// </summary>
    /// <param name="id">Уникальный идентификатор лота</param>
    /// <returns>Лот</returns>
    public async Task<Lot> SelectAsync(Guid id)
    {
        var lot = await _pgsqlHandler.ReadAsync<Lot>(
            "Lot.SelectLot",
            "id",
            id,
            dataReader => new Lot(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetGuid("auctionId"),
                dataReader.GetDecimal("startPrice"),
                dataReader.GetDecimal("buyoutPrice"),
                dataReader.GetDecimal("betStep"),
                (State)dataReader.GetInt32("state")));

        var bets = await _pgsqlHandler.ReadManyByParameterAsync(
            "Bet.SelectBetsByLot",
            dataReader => new Bet
            {
                Id = dataReader.GetGuid("id"),
                Value = dataReader.GetDecimal("value"),
                LotId = dataReader.GetGuid("lotId"),
                UserId = dataReader.GetGuid("userId"),
                DateTime = dataReader.GetDateTime("dateTime")
            },
            new KeyValuePair<string, object>("lotId", lot.Id));

        var images = await _pgsqlHandler.ReadManyByParameterAsync(
            "Image.SelectImagesByLot",
            dataReader => new Image
            {
                Id = dataReader.GetGuid("id"),
                LotId = dataReader.GetGuid("lotId"),
                Path = dataReader.GetString("path")
            },
            new KeyValuePair<string, object>("lotId", lot.Id));

        lot.SetImages(images);
        lot.SetBets(bets);

        return lot;
    }

    /// <summary>
    /// Запрос на получение списка Лотов
    /// </summary>
    /// <returns>Список лотов</returns>
    public async Task<IReadOnlyCollection<Lot>> SelectManyAsync()
    {
        var lots = await _pgsqlHandler.ReadManyAsync<Lot>("Lot.SelectLots",
            dataReader => new Lot(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetGuid("auctionId"),
                dataReader.GetDecimal("startPrice"),
                dataReader.GetDecimal("buyoutPrice"),
                dataReader.GetDecimal("betStep"),
                (State)dataReader.GetInt32("state")));

        var newLots = new List<Lot>();

        foreach (var lot in lots)
        {
            var bets = await _pgsqlHandler.ReadManyByParameterAsync(
                "Bet.SelectBetsByLot",
                dataReader => new Bet
                {
                    Id = dataReader.GetGuid("id"),
                    Value = dataReader.GetDecimal("value"),
                    LotId = dataReader.GetGuid("lotId"),
                    UserId = dataReader.GetGuid("userId"),
                    DateTime = dataReader.GetDateTime("dateTime")
                },
                new KeyValuePair<string, object>("lotId", lot.Id));

            var images = await _pgsqlHandler.ReadManyByParameterAsync(
                "Image.SelectImagesByLot",
                dataReader => new Image
                {
                    Id = dataReader.GetGuid("id"),
                    LotId = dataReader.GetGuid("lotId"),
                    Path = dataReader.GetString("path")
                },
                new KeyValuePair<string, object>("lotId", lot.Id));

            lot.SetImages(images);
            lot.SetBets(bets);

            newLots.Add(lot);
        }

        return newLots;
    }

    /// <summary>
    /// Запрос на получение списка Лотов по аукциону
    /// </summary>
    /// <returns>Список лотов</returns>
    public async Task<IReadOnlyCollection<Lot>> SelectManyByAuctionAsync(Guid auctionId)
    {
        var lots = await _pgsqlHandler.ReadManyByParameterAsync<Lot>("Lot.SelectLotByAuction",
            dataReader => new Lot(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetGuid("auctionId"),
                dataReader.GetDecimal("startPrice"),
                dataReader.GetDecimal("buyoutPrice"),
                dataReader.GetDecimal("betStep"),
                (State)dataReader.GetInt32("state")),
            new KeyValuePair<string, object>("auctionId", auctionId));

        var newLots = new List<Lot>();

        foreach (var lot in lots)
        {
            var bets = await _pgsqlHandler.ReadManyByParameterAsync(
                "Bet.SelectBetsByLot",
                dataReader => new Bet
                {
                    Id = dataReader.GetGuid("id"),
                    Value = dataReader.GetDecimal("value"),
                    LotId = dataReader.GetGuid("lotId"),
                    UserId = dataReader.GetGuid("userId"),
                    DateTime = dataReader.GetDateTime("dateTime")
                },
                new KeyValuePair<string, object>("lotId", lot.Id));

            var images = await _pgsqlHandler.ReadManyByParameterAsync(
                "Image.SelectImagesByLot",
                dataReader => new Image
                {
                    Id = dataReader.GetGuid("id"),
                    LotId = dataReader.GetGuid("lotId"),
                    Path = dataReader.GetString("path")
                },
                new KeyValuePair<string, object>("lotId", lot.Id));

            lot.SetImages(images);
            lot.SetBets(bets);

            newLots.Add(lot);
        }

        return newLots;
    }

    /// <summary>
    /// Запрос на обновление Лота
    /// </summary>
    /// <param name="entity">Лот</param>
    /// <returns>Лот</returns>
    public async Task UpdateAsync(Lot entity)
    {
        await _pgsqlHandler.ExecuteAsync("Lot.UpdateLot",
            new KeyValuePair<string, object>("id", entity.Id),
            new KeyValuePair<string, object>("name", entity.Name),
            new KeyValuePair<string, object>("description", entity.Description),
            new KeyValuePair<string, object>("betStep", entity.BetStep),
            new KeyValuePair<string, object>("state", (int)entity.State));
    }

    /// <summary>
    /// Запрос на удаление Лота
    /// </summary>
    /// <param name="id">Уникальный идентификатор лота</param>
    /// <returns>True или False</returns>
    public async Task DeleteAsync(Guid id)
    {
        var lot = await SelectAsync(id);

        await _pgsqlHandler.ExecuteAsync("Image.DeleteImage",
            new KeyValuePair<string, object>("lotId", id));

        await _pgsqlHandler.ExecuteAsync("Bet.DeleteBet",
            new KeyValuePair<string, object>("lotId", id));

        await _pgsqlHandler.ExecuteAsync("Lot.DeleteLot",
            new KeyValuePair<string, object>("id", id));

        await _fileHandler.DeleteImagesFromHostAsync(lot.Name);
    }
}