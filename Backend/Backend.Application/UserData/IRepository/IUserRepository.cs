using Backend.Application.Repositories;
using Backend.Domain.Entity;

namespace Backend.Application.UserData.IRepository;

/// <summary>
/// Интерфейс репозитория Пользователя
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Запрос на получение пользователя по имени
    /// </summary>
    /// <param name="name">Имя</param>
    /// <returns>Пользователь</returns>
    public Task<User> SelectByNameAsync(string name);
}