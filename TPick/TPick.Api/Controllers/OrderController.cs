using CsMicro.Cqrs.Commands;
using CsMicro.Cqrs.Queries;
using CsMicro.Persistence.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPick.App.Commands;
using TPick.App.Queries;
using TPick.Domain.Aggregates;

namespace TPick.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;

    public OrderController(ICommandBus commandBus, IQueryBus queryBus)
    {
        _commandBus = commandBus;
        _queryBus = queryBus;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> InitOrder([FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var id = Guid.Empty;
        command.OnOrderId = x => id = x;
        var result = await _commandBus.SendAsync(command, cancellationToken);

        return result.IsSuccess ? Ok(new {Id = id}) : BadRequest();
    }

    [HttpGet]
    [Route("{orderId:guid}")]
    public async Task<IActionResult> GetOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _queryBus.SendAsync(new GetOrderDetailsQuery
        {
            Id = orderId
        }, cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch]
    [Route("{orderId:guid}/confirmation")]
    public async Task<IActionResult> PatchOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _commandBus.SendAsync(new ConfirmOrderCommand() {OrderId = orderId}, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest();
    }

    [HttpPost]
    [Route("{orderId:guid}/sub-orders")]
    public async Task<IActionResult> SubmitSubOrder([FromRoute] Guid orderId, [FromBody] SubmitSubOrderCommand command,
        CancellationToken cancellationToken)
    {
        command.OrderId = orderId;
        var result = await _commandBus.SendAsync(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest();
    }

    [HttpDelete]
    [Route("{orderId:guid}/sub-orders/{ownerId:guid}")]
    public async Task<IActionResult> RemoveSubOrder([FromRoute] Guid orderId, [FromRoute] Guid ownerId,
        CancellationToken cancellationToken)
    {
        var result = await _commandBus.SendAsync(new RemoveSubOrderCommand {OrderId = orderId, OwnerId = ownerId},
            cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest();
    }
}