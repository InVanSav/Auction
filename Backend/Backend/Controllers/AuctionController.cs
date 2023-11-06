using Backend.Application.AuctionData.Dto;
using Backend.Application.AuctionData.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер аукциона
/// </summary>
[Authorize]
[ApiController]
[Route("api/auction")]
public class AuctionController : ControllerBase
{
    /// <summary>
    /// Обработчик удаления
    /// </summary>
    private readonly DeleteAuctionHandler _deleteHandler;

    /// <summary>
    /// Обработчик получения списка аукционов
    /// </summary>
    private readonly GetAuctionsHandler _getHandler;

    /// <summary>
    /// Обработчик создания аукциона
    /// </summary>
    private readonly CreateAuctionHandler _createHandler;

    /// <summary>
    /// Обработчик установки даты конца
    /// </summary>
    private readonly SetDateEndAuctionHandler _setDateEndHandler;

    /// <summary>
    /// Обработчик установки даты начала
    /// </summary>
    private readonly SetDateStartAuctionHandler _setDateStartHandler;

    /// <summary>
    /// Обработчик обновления аукциона
    /// </summary>
    private readonly UpdateAuctionHandler _updateHandler;

    /// <summary>
    /// Обработчик смены статуса
    /// </summary>
    private readonly ChangeAuctionStatusHandler _changeAuctionStatusHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="deleteHandler">Обработчик удаления</param>
    /// <param name="getHandler">Обработчик получения списка аукционов</param>
    /// <param name="createHandler">Обработчик создания аукциона</param>
    /// <param name="setDateEndHandler">Обработчик установки даты конца</param>
    /// <param name="setDateStartHandler">Обработчик установки даты начала</param>
    /// <param name="updateHandler">Обработчик обновления аукциона</param>
    /// <param name="changeAuctionStatusHandler">Обработчик смены статуса</param>
    public AuctionController(DeleteAuctionHandler deleteHandler, GetAuctionsHandler getHandler,
        CreateAuctionHandler createHandler, SetDateEndAuctionHandler setDateEndHandler,
        SetDateStartAuctionHandler setDateStartHandler, UpdateAuctionHandler updateHandler,
        ChangeAuctionStatusHandler changeAuctionStatusHandler)
    {
        _deleteHandler = deleteHandler;
        _getHandler = getHandler;
        _createHandler = createHandler;
        _setDateEndHandler = setDateEndHandler;
        _setDateStartHandler = setDateStartHandler;
        _updateHandler = updateHandler;
        _changeAuctionStatusHandler = changeAuctionStatusHandler;
    }

    /// <summary>
    /// Запрос на удаление аукциона
    /// </summary>
    /// <param name="id">Уникальный идентификатор аукциона</param>
    [HttpDelete("delete")]
    public async Task DeleteAuctionAsync([FromQuery] Guid id)
    {
        await _deleteHandler.DeleteAuctionAsync(id);
    }

    /// <summary>
    /// Запрос на получение аукциона по уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный индентификатор аукциона</param>
    [HttpGet("get-by-id")]
    public async Task<Result<AuctionDto>> GetAuctionByIdAsync([FromQuery] Guid id)
    {
        return await _getHandler.GetAuctionByIdAsync(id);
    }

    /// <summary>
    /// Запрос на получение списка аукционов
    /// </summary>
    /// <returns>Список аукционов</returns>
    [HttpGet("get-list")]
    public async Task<Result<IReadOnlyCollection<AuctionDto>>> GetAuctionsAsync()
    {
        return await _getHandler.GetAuctions();
    }

    /// <summary>
    /// Запрос на создание аукциона
    /// </summary>
    /// <param name="entity">Аукцион</param>
    [HttpPost("create")]
    public async Task CreateAuctionAsync([FromBody] CreateAuctionDto entity)
    {
        await _createHandler.CreateAuctionAsync(entity);
    }

    /// <summary>
    /// Запрос на установку даты завершения аукциона
    /// </summary>
    /// <param name="id">Уникальный индентификатор аукциона</param>
    [HttpPut("date-end")]
    public async Task SetDateEndAsync([FromQuery] Guid id)
    {
        await _setDateEndHandler.SetDateEndAuctionAsync(id);
    }

    /// <summary>
    /// Запрос на установку даты начала аукциона
    /// </summary>
    /// <param name="id">Уникальный индентификатор аукциона</param>
    [HttpPut("date-start")]
    public async Task SetDateStartAsync([FromQuery] Guid id)
    {
        await _setDateStartHandler.SetDateStartAuctionAsync(id);
    }

    /// <summary>
    /// Запрос на обновление аукциона
    /// </summary>
    /// <param name="entity">Аукцион</param>
    [HttpPut("update")]
    public async Task UpdateAsync([FromBody] AuctionDto entity)
    {
        await _updateHandler.UpdateAuctionAsync(entity);
    }

    /// <summary>
    /// Запрос наизменение статуса аукциона
    /// </summary>
    /// <param name="newStatus">ID лота и новый статус</param>
    [HttpPut("change-status")]
    public async Task ChangeAuctionStatusAsync([FromBody] ChangeStatusDto newStatus)
    {
        await _changeAuctionStatusHandler.ChangeAuctionStatus(newStatus);
    }
}