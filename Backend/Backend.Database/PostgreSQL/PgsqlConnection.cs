namespace Backend.Database.PostgreSQL;

/// <summary>
/// Класс строки подключения базы данных PostgreSQL
/// </summary>
public class PgsqlConnection
{
    /// <summary>
    /// Название сервера
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// Номер порта сервера
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Название базы данных
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// Имя владельца базой данных
    /// </summary>
    public string? User { get; set; }

    /// <summary>
    /// Пароль базы данных
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Получить строку подключения к базе данных
    /// </summary>
    /// <returns>Строку подключения к базе данных PostgreSQL</returns>
    public string GetConnectionString()
    {
        return $"Server={Server};Port={Port};Database={Database};User Id={User};Password={Password};";
    }
}