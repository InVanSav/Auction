using Backend.Application.UserData.Dto;
using Backend.Application.UserData.IRepository;

namespace Backend.Application.UserData.UseCases;

/// <summary>
/// Получение списка пользователей
/// </summary>
public class GetUsersHandler
{
    /// <summary>
    /// Репозиторий пользователя
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователя</param>
    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Получить список пользователей
    /// </summary>
    /// <returns>Список моделей пользователя</returns>
    public async Task<IReadOnlyCollection<UserDto>> GetUsersAsync()
    {
        var users = await _userRepository.SelectManyAsync();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        }).ToList();
    }
}