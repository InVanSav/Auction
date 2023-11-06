namespace Backend.Domain.Entity;

/// <summary>
/// Ставка
/// </summary>
public class Bet
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Значение ставки
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// Уникальный идентификатор лота
    /// </summary>
    public Guid LotId { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Время ставки
    /// </summary>
    public DateTime DateTime { get; init; }
}