namespace Backend.Application.AuctionData.Dto;

/// <summary>
/// Класс для создания аукциона
/// </summary>
public record CreateAuctionDto
{
    /// <summary>
    /// Название аукциона
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Описание аукциона
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя-создателя
    /// </summary>
    public Guid AuthorId { get; init; }
}