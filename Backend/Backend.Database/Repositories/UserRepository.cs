using System.Data;
using Backend.Application.UserData.IRepository;
using Backend.Database.PostgreSQL;
using Backend.Domain.Entity;

namespace Backend.Database.Repositories;

/// <summary>
/// Репозиторий Пользователя
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// Обработчик запросов к базе данных
    /// </summary>
    private readonly PgsqlHandler _pgsqlHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="pgsqlHandler">Обработчик запросов к базе данных</param>
    public UserRepository(PgsqlHandler pgsqlHandler)
    {
        _pgsqlHandler = pgsqlHandler;
    }

    /// <summary>
    /// Запрос на добавление Пользователя
    /// </summary>
    /// <param name="entity">Пользователь</param>
    /// <returns>True или False</returns>
    public async Task CreateAsync(User entity)
    {
        await _pgsqlHandler.ExecuteAsync("User.InsertUser",
            new KeyValuePair<string, object>("id", entity.Id),
            new KeyValuePair<string, object>("name", entity.Name),
            new KeyValuePair<string, object>("email", entity.Email),
            new KeyValuePair<string, object>("password", entity.Password));
    }

    /// <summary>
    /// Запрос на получение пользователя
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя</param>
    /// <returns>Пользователь</returns>
    public async Task<User?> SelectAsync(Guid id)
    {
        return await _pgsqlHandler.ReadAsync<User>(
            "User.SelectUser",
            "id",
            id,
            dataReader => new User(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("email"),
                dataReader.GetString("password")));
    }

    /// <summary>
    /// Запрос на получение пользователя по имени
    /// </summary>
    /// <param name="email">Почта пользователя</param>
    /// <returns>Пользователь</returns>
    public async Task<User> SelectByNameAsync(string email)
    {
        return await _pgsqlHandler.ReadAsync<User>(
            "User.SelectUserByName",
            "email",
            email,
            dataReader => new User(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("email"),
                dataReader.GetString("password")));
    }

    /// <summary>
    /// Запрос на получение списка Пользователей
    /// </summary>
    /// <returns>Список пользователей</returns>
    public async Task<IReadOnlyCollection<User>> SelectManyAsync()
    {
        return await _pgsqlHandler.ReadManyAsync<User>("User.SelectUsers",
            dataReader => new User(
                dataReader.GetGuid("id"),
                dataReader.GetString("name"),
                dataReader.GetString("email"),
                dataReader.GetString("password")));
    }

    /// <summary>
    /// Запрос на обновление Пользователя
    /// </summary>
    /// <param name="entity">Пользователь</param>
    /// <returns>Пользователь</returns>
    public async Task UpdateAsync(User entity)
    {
        await _pgsqlHandler.ExecuteAsync("User.UpdateUser",
            new KeyValuePair<string, object>("id", entity.Id),
            new KeyValuePair<string, object>("name", entity.Name),
            new KeyValuePair<string, object>("email", entity.Email),
            new KeyValuePair<string, object>("password", entity.Password));
    }

    /// <summary>
    /// Запрос на удаление Пользователя
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя</param>
    /// <returns>True или False</returns>
    public async Task DeleteAsync(Guid id)
    {
        await _pgsqlHandler.ExecuteAsync("User.DeleteUser",
            new KeyValuePair<string, object>("id", id));
    }
}