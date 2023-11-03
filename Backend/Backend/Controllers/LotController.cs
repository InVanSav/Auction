using Backend.Application.LotData.Dto;
using Backend.Application.LotData.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер лота
/// </summary>
[Authorize]
[ApiController]
[Route("api/lot/")]
public class LotController : ControllerBase
{
    /// <summary>
    /// Обработчик удаления лота
    /// </summary>
    private readonly DeleteLotHandler _deleteHandler;

    /// <summary>
    /// Обработчик создания лота
    /// </summary>
    private readonly CreateLotHandler _createHandler;

    /// <summary>
    /// Обработчик выкупа лота
    /// </summary>
    private readonly BuyoutLotHandler _buyoutHandler;

    /// <summary>
    /// Обработчик изменения статуса лота
    /// </summary>
    private readonly ChangeLotStatusHandler _changeStatusHandler;

    /// <summary>
    /// Обработчик ставки
    /// </summary>
    private readonly DoBetHandler _doBetHandler;

    /// <summary>
    /// Обработчик обновления лота
    /// </summary>
    private readonly UpdateLotHandler _updateHandler;

    /// <summary>
    /// Обработчик получения списка лотов
    /// </summary>
    private readonly GetLotsHandler _getLotsHandler;

    /// <summary>
    /// Обработчик получения лотов по аукциону
    /// </summary>
    private readonly GetLotsByAuctionHandler _getLotsByAuctionHandler;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="deleteHandler">Обработчик удаления лота</param>
    /// <param name="createHandler">Обработчик создания лота</param>
    /// <param name="buyoutHandler">Обработчик выкупа лота</param>
    /// <param name="changeStatusHandler">Обработчик изменения статуса лота</param>
    /// <param name="doBetHandler">Обработчик ставки</param>
    /// <param name="updateHandler">Обработчик обновления лота</param>
    /// <param name="getLotsHandler">Обработчик получения списка лотов</param>
    /// <param name="getLotsByAuctionHandler">Обработчик получения лотов по аукциону</param>
    public LotController(DeleteLotHandler deleteHandler, CreateLotHandler createHandler, BuyoutLotHandler buyoutHandler,
        ChangeLotStatusHandler changeStatusHandler, DoBetHandler doBetHandler, UpdateLotHandler updateHandler,
        GetLotsHandler getLotsHandler, GetLotsByAuctionHandler getLotsByAuctionHandler)
    {
        _deleteHandler = deleteHandler;
        _createHandler = createHandler;
        _buyoutHandler = buyoutHandler;
        _changeStatusHandler = changeStatusHandler;
        _doBetHandler = doBetHandler;
        _updateHandler = updateHandler;
        _getLotsHandler = getLotsHandler;
        _getLotsByAuctionHandler = getLotsByAuctionHandler;
    }

    /// <summary>
    /// Запрос на удаление лота
    /// </summary>
    /// <param name="id">Уникальный индентификатор лота</param>
    [HttpDelete("delete/{id:guid}")]
    public async Task DeleteLotAsync(Guid id)
    {
        await _deleteHandler.DeleteLotAsync(id);
    }

    /// <summary>
    /// Запрос на создание лота
    /// </summary>
    /// <param name="formCollection">Изображения лота</param>
    [HttpPost("create")]
    public async Task CreateLotASync([FromForm] IFormCollection formCollection)
    {
        await _createHandler.CreateLotAsync(formCollection);
    }

    /// <summary>
    /// Запрос на выкуп лота
    /// </summary>
    /// <param name="buyoutDto">Выкупить лот</param>
    [HttpPut("buyout")]
    public async Task BuyoutLotAsync(BuyoutDto buyoutDto)
    {
        await _buyoutHandler.BuyoutLotAsync(buyoutDto);
    }

    /// <summary>
    /// Запрос на изменение статуса лота
    /// </summary>
    /// <param name="changeStatusDto">Изменить статус лота</param>
    [HttpPut("change-status")]
    public async Task ChangeLotStatusAsync(ChangeLotStatusDto changeStatusDto)
    {
        await _changeStatusHandler.ChangeLotStatusAsync(changeStatusDto);
    }

    /// <summary>
    /// Запрос на ставку
    /// </summary>
    /// <param name="doBetDto">Сделать ставку</param>
    [HttpPut("do-bet")]
    public async Task DoBetAsync(DoBetDto doBetDto)
    {
        await _doBetHandler.DoBetAsync(doBetDto);
    }

    /// <summary>
    ///  Запрос на обновление лота
    /// </summary>
    /// <param name="lot"></param>
    [HttpPut("update")]
    public async Task UpdateLotAsync([FromBody] LotDto lot)
    {
        await _updateHandler.UpdateLotAsync(lot);
    }

    /// <summary>
    /// Запрос на получение списка лотов
    /// </summary>
    /// <returns>список лотов</returns>
    [HttpGet("get-list")]
    public Task<IReadOnlyCollection<LotDto>> GetLotsAsync()
    {
        return _getLotsHandler.GetLots();
    }

    /// <summary>
    /// Запрос на получение списка лотов по аукциону
    /// </summary>
    /// <param name="auctionId">Уникальный индентификатор аукциона</param>
    /// <returns>список лотов</returns>
    [HttpGet("get-list-by-auction/{auctionId:guid}")]
    public Task<IReadOnlyCollection<LotDto>> GetLotsByAuctionAsync(Guid auctionId)
    {
        return _getLotsByAuctionHandler.GetLotsByAuction(auctionId);
    }
}