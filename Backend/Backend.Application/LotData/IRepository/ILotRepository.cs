using Backend.Application.Repositories;
using Backend.Domain.Entity;

namespace Backend.Application.LotData.IRepository;

/// <summary>
/// Интерфейс репозитория Лота
/// </summary>
public interface ILotRepository : IBaseRepository<Lot>
{
    Task<IReadOnlyCollection<Lot>> SelectManyByAuctionAsync(Guid auctionId);
}