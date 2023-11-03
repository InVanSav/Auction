using System.Data;
using Backend.Application.AuctionData.IRepository;
using Backend.Database.PostgreSQL;
using Backend.Domain.Entity;
using Backend.Domain.Enum;

namespace Backend.Database.Repositories;

/// <summary>
/// Репозиторий Аукциона
/// </summary>
public class AuctionRepository : IAuctionRepository
{
    /// <summary>
    /// Обработчик запросов к базе данных
    /// </summary>
    private readonly PgsqlHandler _pgsqlHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="pgsqlHandler">Обработчик запросов к базе данных</param>
    public AuctionRepository(PgsqlHandler pgsqlHandler)
    {
        _pgsqlHandler = pgsqlHandler;
    }

    /// <summary>
    /// Запрос на создание Аукциона
    /// </summary>
    /// <param name="entity">Аукцион</param>
    /// <returns>True или False</returns>
    public async Task CreateAsync(Auction entity)
    {
        await _pgsqlHandler.ExecuteAsync("Auction.InsertAuction",
            new KeyValuePair<string, object>("id", entity.Id),
            new KeyValuePair<string, object>("name", entity.Name!),
            new KeyValuePair<string, object>("description", entity.Description!),
            new KeyValuePair<string, object>("state", (int)entity.State),
            new KeyValuePair<string, object>("authorId", entity.AuthorId));
    }

    /// <summary>
    /// Запрос на получение Аукциона
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    /// <returns>Аукцион</returns>
    public async Task<Auction> SelectAsync(Guid id)
    {
        var auction = await _pgsqlHandler.ReadAsync<Auction>(
            "Auction.SelectAuction",
            "id",
            id,
            dataReader => new Auction(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetDateTime("dateStart"),
                dataReader.GetDateTime("dateEnd"),
                dataReader.GetGuid("authorId"),
                (State)dataReader.GetInt32("state")));

        var lots = await _pgsqlHandler.ReadManyByParameterAsync<Lot>(
            "Lot.SelectLotByAuction",
            dataReader => new Lot(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetGuid("auctionId"),
                dataReader.GetDecimal("startPrice"),
                dataReader.GetDecimal("buyoutPrice"),
                dataReader.GetDecimal("betStep"),
                (State)dataReader.GetInt32("state")),
            new KeyValuePair<string, object>("auctionId", id));

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

            auction.AddLot(lot, images, bets);
        }

        return auction;
    }

    /// <summary>
    /// Запрос на получение списка Аукционов
    /// </summary>
    /// <returns>Список аукционов</returns>
    public async Task<IReadOnlyCollection<Auction>> SelectManyAsync()
    {
        var auctions = await _pgsqlHandler.ReadManyAsync<Auction>("Auction.SelectAuctions",
            dataReader => new Auction(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetDateTime("dateStart"),
                dataReader.GetDateTime("dateEnd"),
                dataReader.GetGuid("authorId"),
                (State)dataReader.GetInt32("state")));

        var newAuctions = new List<Auction>();

        foreach (var auction in auctions)
        {
            var lots = await _pgsqlHandler.ReadManyByParameterAsync<Lot>(
                "Lot.SelectLotByAuction",
                dataReader => new Lot(
                    dataReader.GetGuid("id"),
                    dataReader.GetString("name"),
                    dataReader.GetString("description"),
                    dataReader.GetGuid("auctionId"),
                    dataReader.GetDecimal("startPrice"),
                    dataReader.GetDecimal("buyoutPrice"),
                    dataReader.GetDecimal("betStep"),
                    (State)dataReader.GetInt32("state")),
                new KeyValuePair<string, object>("auctionId", auction.Id));

            if (lots.Count == 0) return auctions;

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

                auction.AddLot(lot, images, bets);
                newAuctions.Add(auction);
            }
        }

        return newAuctions;
    }

    /// <summary>
    /// Запрос на обновление Аукциона
    /// </summary>
    /// <param name="entity">Аукцион</param>
    /// <returns>Аукцион</returns>
    public async Task UpdateAsync(Auction entity)
    {
        await _pgsqlHandler.ExecuteAsync("Auction.UpdateAuction",
            new KeyValuePair<string, object>("id", entity.Id),
            new KeyValuePair<string, object>("name", entity.Name!),
            new KeyValuePair<string, object>("description", entity.Description!),
            new KeyValuePair<string, object>("dateStart", entity.DateStart),
            new KeyValuePair<string, object>("dateEnd", entity.DateEnd),
            new KeyValuePair<string, object>("state", (int)entity.State));

        foreach (var lot in entity.Lots.Values)
        {
            await _pgsqlHandler.ExecuteAsync("Lot.UpdateLot",
                new KeyValuePair<string, object>("id", lot.Id),
                new KeyValuePair<string, object>("name", lot.Name),
                new KeyValuePair<string, object>("description", lot.Description),
                new KeyValuePair<string, object>("startPrice", lot.StartPrice),
                new KeyValuePair<string, object>("buyoutPrice", lot.BuyoutPrice),
                new KeyValuePair<string, object>("betStep", lot.BetStep),
                new KeyValuePair<string, object>("state", (int)lot.State));

            foreach (var bet in lot.Bets)
            {
                await _pgsqlHandler.ExecuteAsync("Bet.InsertBet",
                    new KeyValuePair<string, object>("id", bet.Id),
                    new KeyValuePair<string, object>("value", bet.Value),
                    new KeyValuePair<string, object>("lotId", bet.LotId),
                    new KeyValuePair<string, object>("userId", bet.UserId),
                    new KeyValuePair<string, object>("dateTime", bet.DateTime));
            }
        }
    }

    /// <summary>
    /// Запрос на удаление Аукциона
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    /// <returns>True или False</returns>
    public async Task DeleteAsync(Guid id)
    {
        await _pgsqlHandler.ExecuteAsync("Auction.DeleteAuction",
            new KeyValuePair<string, object>("id", id));

        var lots = await _pgsqlHandler.ReadManyByParameterAsync<Lot>(
            "Lot.SelectLotByAuction",
            dataReader => new Lot(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetGuid("auctionId"),
                dataReader.GetDecimal("startPrice"),
                dataReader.GetDecimal("buyoutPrice"),
                dataReader.GetDecimal("betStep"),
                (State)dataReader.GetInt32("state")),
            new KeyValuePair<string, object>("auctionId", id));

        foreach (var lot in lots)
        {
            await _pgsqlHandler.ExecuteAsync("Image.DeleteImage",
                new KeyValuePair<string, object>("lotId", lot.Id));

            await _pgsqlHandler.ExecuteAsync("Bet.DeleteBet",
                new KeyValuePair<string, object>("lotId", lot.Id));
        }

        await _pgsqlHandler.ExecuteAsync("Lot.DeleteLot",
            new KeyValuePair<string, object>("auctionId", id));
    }
}