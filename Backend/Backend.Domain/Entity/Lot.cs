using System.Collections.Specialized;
using Backend.Domain.Enum;
using FluentResults;

namespace Backend.Domain.Entity;

/// <summary>
/// Лот
/// </summary>
public class Lot
{
    /// <summary>
    /// Примитив синхронизации потоков
    /// </summary>
    private readonly object _locker = new();

    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название лота
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Описание лота
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Уникальный идентификатор аукциона лота
    /// </summary>
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Стартовая цена лота
    /// </summary>
    public decimal StartPrice { get; init; }

    /// <summary>
    /// Цена выкупа лота
    /// </summary>
    public decimal BuyoutPrice { get; private set; }

    /// <summary>
    /// Шаг ставки лота
    /// </summary>
    public decimal BetStep { get; private set; }

    /// <summary>
    /// Ставки лота
    /// </summary>
    private readonly List<Bet> _bets = new();

    public IReadOnlyCollection<Bet> Bets => _bets;

    /// <summary>
    /// Изображения лота
    /// </summary>
    private readonly List<Image> _images = new();

    public IReadOnlyCollection<Image> Images => _images;

    /// <summary>
    /// Статус лота
    /// </summary>
    public State State { get; private set; } = State.Awaiting;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="name">Название лота</param>
    /// <param name="description">Описание лота</param>
    /// <param name="auctionId">Уникальный идентификатор аукциона лота</param>
    /// <param name="startPrice">Начальная цена</param>
    /// <param name="betStep">Шаг ставки лота</param>
    /// <param name="images">Изображения лота</param>
    public Lot(string name, string description, Guid auctionId, decimal startPrice, decimal betStep,
        IEnumerable<Image> images)
    {
        Name = name;
        Description = description;
        AuctionId = auctionId;
        StartPrice = startPrice;
        BetStep = betStep;

        SetImages(images);
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="id">Уникальный идентификатор лота</param>
    /// <param name="name">Название лота</param>
    /// <param name="description">Описание лота</param>
    /// <param name="auctionId">Уникальный идентификатор аукциона лота</param>
    /// <param name="startPrice">Начальная цена</param>
    /// <param name="buyoutPrice">Цена выкупа лота</param>
    /// <param name="betStep">Шаг ставки лота</param>
    /// <param name="state">Состояние лота</param>
    public Lot(Guid id, string name, string description, Guid auctionId, decimal startPrice, decimal buyoutPrice,
        decimal betStep, State state)
    {
        Id = id;
        Name = name;
        Description = description;
        AuctionId = auctionId;
        StartPrice = startPrice;
        BuyoutPrice = buyoutPrice;
        BetStep = betStep;
        State = state;
    }

    /// <summary>
    /// Обновить информацию о лоте
    /// </summary>
    /// <param name="name">Название лота</param>
    /// <param name="description">Описание лота</param>
    /// <param name="betStep">Шаг ставки</param>
    /// <param name="images">Изображения лота</param>
    /// <returns>Успех или неудача</returns>
    public Result UpdateInformation(string name, string description, decimal betStep, IEnumerable<Image> images)
    {
        Name = name;
        Description = description;
        BetStep = betStep;

        SetImages(images);

        return Result.Ok();
    }

    /// <summary>
    /// Установить изображения лота
    /// </summary>
    /// <param name="images">Изображения</param>
    /// <returns>Успех или неудача</returns>
    public Result SetImages(IEnumerable<Image> images)
    {
        _images.Clear();

        foreach (var image in images)
        {
            _images.Add(new Image
            {
                Id = image.Id,
                LotId = image.LotId,
                Path = image.Path
            });
        }

        return Result.Ok();
    }

    /// <summary>
    /// Установить ствавки лота
    /// </summary>
    /// <param name="bets">Ставки</param>
    /// <returns>Успех или неудача</returns>
    public Result SetBets(IEnumerable<Bet> bets)
    {
        _bets.Clear();

        foreach (var bet in bets)
        {
            _bets.Add(new Bet
            {
                Id = bet.Id,
                Value = bet.Value,
                LotId = bet.LotId,
                UserId = bet.UserId,
                DateTime = bet.DateTime
            });
        }

        return Result.Ok();
    }

    /// <summary>
    /// Установить цену выкупа лота
    /// </summary>
    /// <returns>Успех или неудача</returns>
    public Result SetBuyoutPrice()
    {
        BuyoutPrice = _bets.Count > 0 ? StartPrice + _bets.Max(b => b.Value) : 0;
        return Result.Ok();
    }

    /// <summary>
    /// Попытаться сделать ставку
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя</param>
    /// <returns>Успех или неудача</returns>
    public Result TryDoBet(Guid userId)
    {
        lock (_locker)
        {
            var value = _bets.Count > 0
                ? _bets.Max(b => b.Value) + BetStep
                : BetStep;

            var bet = new Bet
            {
                Id = Guid.NewGuid(),
                Value = value,
                LotId = Id,
                UserId = userId,
                DateTime = DateTime.Now
            };

            _bets.Add(bet);
        }

        return Result.Ok();
    }

    /// <summary>
    /// Изменить статус лота
    /// </summary>
    /// <param name="state">Состояние лота</param>
    /// <returns>Успех или неудача</returns>
    public Result ChangeStatus(State state)
    {
        if (state is State.Completed) SetBuyoutPrice();

        State = state;

        return Result.Ok();
    }
}