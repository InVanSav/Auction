using Backend.Domain.Enum;

namespace Backend.Application.AuctionData.Dto;

/// <summary>
/// Класс для смены статуса аукциона
/// </summary>
public record ChangeStatusDto
{
    /// <summary>
    /// Уникальный идентификтор аукциона
    /// </summary>
    public Guid AuctionId { get; init; }

    /// <summary>
    /// Новый статус
    /// </summary>
    public State State { get; init; }
}