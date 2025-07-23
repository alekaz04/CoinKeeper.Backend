using CoinKeeper.Common;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Finance;

/// <summary>
/// Контроллер для сущности <see cref="PlannedOperation"/>
/// </summary>
[ApiController]
[Route("api/planned-operations")]
public class PlannedOperationController : AbstractCrudController<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto>
{
    /// <inheritdoc cref="PlannedOperationCrudHandler"/>
    private readonly PlannedOperationCrudHandler _plannedOperationHandler;

    public PlannedOperationController(AbstractCrudHandler<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto> handler) : base(handler)
    {
        _plannedOperationHandler = (PlannedOperationCrudHandler)handler;
    }

    /// <summary>
    /// Ручное выполнение планового платежа
    /// </summary>
    /// <param name="id">Идентификатор планового платежа</param>
    /// <param name="executeDto">Параметры выполнения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная операция</returns>
    [HttpPost("{id:guid}/execute")]
    public async Task<ActionResult<OperationReadDto>> ExecutePlannedOperation(
        Guid id,
        [FromBody] PlannedOperationExecuteDto executeDto,
        CancellationToken cancellationToken = default)
    {
        var result = await _plannedOperationHandler.ExecutePlannedOperationAsync(id, executeDto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Приостановка планового платежа
    /// </summary>
    /// <param name="id">Идентификатор планового платежа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Статус планового платежа</returns>
    [HttpPut("{id:guid}/pause")]
    public async Task<ActionResult<PlannedOperationStatusDto>> PausePlannedOperation(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _plannedOperationHandler.PausePlannedOperationAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Возобновление планового платежа
    /// </summary>
    /// <param name="id">Идентификатор планового платежа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Статус планового платежа</returns>
    [HttpPut("{id:guid}/resume")]
    public async Task<ActionResult<PlannedOperationStatusDto>> ResumePlannedOperation(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _plannedOperationHandler.ResumePlannedOperationAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Предварительный просмотр будущих выполнений планового платежа
    /// </summary>
    /// <param name="id">Идентификатор планового платежа</param>
    /// <param name="maxExecutions">Максимальное количество выполнений для просмотра (по умолчанию 10)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список будущих выполнений</returns>
    [HttpGet("{id:guid}/preview")]
    public async Task<ActionResult<PlannedOperationPreviewListDto>> GetPlannedOperationPreview(
        Guid id,
        [FromQuery] int maxExecutions = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _plannedOperationHandler.GetPlannedOperationPreviewAsync(id, maxExecutions, cancellationToken);
        return Ok(result);
    }
}
