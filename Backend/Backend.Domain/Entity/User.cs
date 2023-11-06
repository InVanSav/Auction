namespace Backend.Domain.Entity;

/// <summary>
/// Пользователь
/// </summary>
public class User
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Почта пользователя
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя</param>
    /// <param name="name">Имя пользователя</param>
    /// <param name="email">Почта пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    public User(Guid id, string name, string email, string password)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
    }
}