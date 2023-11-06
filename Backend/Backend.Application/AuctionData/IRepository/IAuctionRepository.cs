using Backend.Application.IRepositories;
using Backend.Domain.Entity;

namespace Backend.Application.AuctionData.IRepository;

/// <summary>
/// Интерфейс репозитория Аукциона
/// </summary>
public interface IAuctionRepository : IBaseRepository<Auction>
{
}