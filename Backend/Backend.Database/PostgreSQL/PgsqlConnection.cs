namespace Backend.Database.PostgreSQL;

/// <summary>
/// Класс строки подключения базы данных PostgreSQL
/// </summary>
public class PgsqlConnection
{
    /// <summary>
    /// Название сервера
    /// </summary>
    public string? Server { private get; init; }

    /// <summary>
    /// Номер порта сервера
    /// </summary>
    public int Port { private get; init; }

    /// <summary>
    /// Название базы данных
    /// </summary>
    public string? Database { private get; init; }

    /// <summary>
    /// Имя владельца базой данных
    /// </summary>
    public string? User { private get; init; }

    /// <summary>
    /// Пароль базы данных
    /// </summary>
    public string? Password { private get; init; }

    /// <summary>
    /// Получить строку подключения к базе данных
    /// </summary>
    /// <returns>Строку подключения к базе данных PostgreSQL</returns>
    internal string GetConnectionString()
    {
        return $"Server={Server};Port={Port};Database={Database};User Id={User};Password={Password};";
    }
}