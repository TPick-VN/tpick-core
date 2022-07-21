using CsMicro.Cqrs.Commands;
using CsMicro.Cqrs.Queries;
using Microsoft.AspNetCore.Mvc;
using TPick.App.Commands;
using TPick.App.Queries;

namespace TPick.Api.Controllers;

[ApiController]
[Route("shops")]
public class ShopController : ControllerBase
{
    private readonly IQueryBus _queryBus;
    private readonly ICommandBus _commandBus;

    public ShopController(IQueryBus queryBus, ICommandBus commandBus)
    {
        _queryBus = queryBus;
        _commandBus = commandBus;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetOneShop([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _queryBus.SendAsync(new GetShopQuery
        {
            Id = id
        }, cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateShop([FromBody] AddOrUpdateShopCommand command,
        CancellationToken cancellationToken)
    {
        var id = Guid.Empty;
        command.OnShopId = shopId => id = shopId;
        var result = await _commandBus.SendAsync(command, cancellationToken);

        return result.IsSuccess ? Ok(new {Id = id}) : BadRequest();
    }
}