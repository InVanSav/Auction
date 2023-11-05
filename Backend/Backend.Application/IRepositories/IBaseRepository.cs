namespace Backend.Application.Repositories;

/// <summary>
/// Базовый репозиторий
/// </summary>
/// <typeparam name="T">Сущность</typeparam>
public interface IBaseRepository<T>
{
    /// <summary>
    /// Создать сущность
    /// </summary>
    /// <param name="entity">Создаваемая сущность</param>
    public Task CreateAsync(T entity);

    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <returns>Сушность</returns>
    public Task<T> SelectAsync(Guid id);

    /// <summary>
    /// Получить сущности
    /// </summary>
    /// <returns>Сушности</returns>
    public Task<IReadOnlyCollection<T>> SelectManyAsync();

    /// <summary>
    /// Удаление сущности
    /// </summary>
    /// <param name="id">Уникальный идентификатор</param>
    public Task DeleteAsync(Guid id);

    /// <summary>
    /// Обновление сущности
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>Сущность</returns>
    public Task UpdateAsync(T entity);
}