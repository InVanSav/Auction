using Backend.Application.LotData.Dto;
using Backend.Domain.Enum;

namespace Backend.Application.AuctionData.Dto;

/// <summary>
/// Аукцион
/// </summary>
public record AuctionDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название аукциона
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Описание аукциона
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Начало аукциона
    /// </summary>
    public DateTime? DateStart { get; init; }

    /// <summary>
    /// Завершение аукциона
    /// </summary>
    public DateTime? DateEnd { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя-создателя
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Статус ставки
    /// </summary>
    public State State { get; init; }

    /// <summary>
    /// Лоты аукциона
    /// </summary>
    public IEnumerable<LotDto> Lots { get; init; } = new List<LotDto>();
}