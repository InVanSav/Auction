using Backend.Domain.Enum;
using FluentResults;

namespace Backend.Domain.Entity;

/// <summary>
/// Аукцион
/// </summary>
public class Auction
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название аукциона
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Описание аукциона
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Начало аукциона
    /// </summary>
    public DateTime DateStart { get; private set; }

    /// <summary>
    /// Завершение аукциона
    /// </summary>
    public DateTime DateEnd { get; private set; }

    /// <summary>
    /// Уникальный идентификатор пользователя-создателя
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Статус ставки
    /// </summary>
    public State State { get; private set; } = State.Awaiting;

    /// <summary>
    /// Лоты аукциона
    /// </summary>
    public Dictionary<Guid, Lot> Lots { get; } = new();

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="name">Название аукциона</param>
    /// <param name="description">Описание аукциона</param>
    /// <param name="authorId">Уникальный идентификатор автора</param>
    public Auction(string? name, string? description, Guid authorId)
    {
        Name = name;
        Description = description;
        AuthorId = authorId;
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    /// <param name="name">Название аукциона</param>
    /// <param name="description">Описание аукциона</param>
    /// <param name="dateStart">Дата начала аукицона</param>
    /// <param name="dateEnd">Дата конца аукциона</param>
    /// <param name="authorId">Уникальный идентификатор автора</param>
    /// <param name="state">Текущее состояние аукциона</param>
    public Auction(Guid id, string? name, string? description, DateTime dateStart, DateTime dateEnd, Guid authorId,
        State state)
    {
        Id = id;
        Name = name;
        Description = description;
        DateStart = dateStart;
        DateEnd = dateEnd;
        AuthorId = authorId;
        State = state;
    }

    /// <summary>
    /// Обновить информацию аукциона
    /// </summary>
    /// <param name="name">Название аукциона</param>
    /// <param name="description">Описание аукциона</param>
    /// <returns>Успех или неудача</returns>
    public Result UpdateInformation(string? name, string? description)
    {
        Name = name;
        Description = description;

        return Result.Ok();
    }

    /// <summary>
    /// Установить дату начала аукциона
    /// </summary>
    /// <returns>Успех или неудача</returns>
    public Result SetDateStart()
    {
        DateStart = DateTime.Now;

        return Result.Ok();
    }

    /// <summary>
    /// Установить дату завершения аукциона
    /// </summary>
    /// <returns>Успех или неудача</returns>
    public Result SetDateEnd()
    {
        DateEnd = DateTime.Now;

        var lotsWithBets = Lots.Values.Where(l => l.Bets.Count > 0).ToArray();
        var maxBetDate = lotsWithBets.SelectMany(l => l.Bets).Max(s => s.DateTime).AddSeconds(60);

        DateEnd = DateEnd >= maxBetDate ? DateEnd : maxBetDate;

        return Result.Ok();
    }

    /// <summary>
    /// Выкупить лот
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <returns>Успех или неудача</returns>
    public Result BuyoutLot(Guid lotId)
    {
        return Lots.TryGetValue(lotId, out var lot)
            ? lot.SetBuyoutPrice()
            : Result.Fail("Лот не найден");
    }

    /// <summary>
    /// Изменить статус аукциона
    /// </summary>
    /// <param name="state">Состояние аукциона</param>
    /// <returns>Успех или неудача</returns>
    public Result ChangeStatus(State state)
    {
        switch (state)
        {
            case State.Canceled or State.Completed:
                DateEnd = DateTime.Now;
                break;
            case State.Running:
                DateStart = DateTime.Now;
                break;
        }

        State = state;
        return Result.Ok();
    }

    /// <summary>
    /// Изменить статус лота
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <param name="state">Новое состояние</param>
    /// <returns>Успех или неудача</returns>
    public Result ChangeLotStatus(Guid lotId, State state)
    {
        if (State is State.Completed) BuyoutLot(lotId);

        return Lots.TryGetValue(lotId, out var lot)
            ? lot.ChangeStatus(state)
            : Result.Fail("Лот не найден");
    }

    /// <summary>
    /// Сделать ставку
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <param name="userId">Уникальный идентификатор пользователя</param>
    /// <returns>Успех или неудача</returns>
    public Result DoBet(Guid lotId, Guid userId)
    {
        return Lots.TryGetValue(lotId, out var lot)
            ? lot.TryDoBet(userId)
            : Result.Fail("Лот не найден");
    }

    /// <summary>
    /// Добавить изображения лота
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <param name="images">Изображения</param>
    /// <returns>Успех или неудача</returns>
    public Result AddLotImages(Guid lotId, IEnumerable<Image> images)
    {
        return Lots.TryGetValue(lotId, out var lot)
            ? lot.SetImages(images)
            : Result.Fail("Лот не найден");
    }

    /// <summary>
    /// Добавить ставки лота
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <param name="bets">Ставки</param>
    /// <returns>Успех или неудача</returns>
    public Result AddLotBets(Guid lotId, IEnumerable<Bet> bets)
    {
        return Lots.TryGetValue(lotId, out var lot)
            ? lot.SetBets(bets)
            : Result.Fail("Лот не найден");
    }

    /// <summary>
    /// Добавить лот
    /// </summary>
    /// <param name="lot">Лот</param>
    /// <param name="images">Изображения лота</param>
    /// <param name="bets">Ставки</param>
    /// <returns>Успех или неудача</returns>
    public Result AddLot(Lot lot, IEnumerable<Image> images, IEnumerable<Bet> bets)
    {
        Lots.Add(lot.Id, lot);
        AddLotImages(lot.Id, images);
        AddLotBets(lot.Id, bets);

        return Result.Ok();
    }

    /// <summary>
    /// Обновить лот
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <param name="name">Новое название</param>
    /// <param name="description">Новое описание</param>
    /// <param name="betStep">Новый шаг ставки</param>
    /// <param name="images">Изображения лота</param>
    /// <returns>Успех или неудача</returns>
    public Result UpdateLot(Guid lotId, string name, string description, decimal betStep,
        IEnumerable<Image> images)
    {
        return Lots.TryGetValue(lotId, out var lot)
            ? lot.UpdateInformation(name, description, betStep, images)
            : Result.Fail("Лот не найден");
    }

    /// <summary>
    /// Удалить лот
    /// </summary>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <returns>Успех или неудача</returns>
    public Result RemoveLot(Guid lotId)
    {
        if (!Lots.ContainsKey(lotId))
            return Result.Fail("Лот не найден");

        Lots.Remove(lotId);

        return Result.Ok();
    }
}