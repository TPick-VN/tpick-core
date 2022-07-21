using CsMicro.Cqrs.Commands;
using CsMicro.Persistence;
using Microsoft.EntityFrameworkCore;
using TPick.Domain.Aggregates;
using TPick.Domain.ValueObjects;

namespace TPick.App.Commands;

public class RemoveSubOrderCommand : ICommand
{
    public Guid OrderId { get; set; }
    public Guid OwnerId { get; init; }
}

public class RemoveSubOrderCommandHandler : ICommandHandler<RemoveSubOrderCommand>
{
    private readonly IGenericRepository<SubOrder, Guid> _subOrderRepo;

    public RemoveSubOrderCommandHandler(IGenericRepository<SubOrder, Guid> subOrderRepo)
    {
        _subOrderRepo = subOrderRepo;
    }

    public async Task<CommandResult> HandleAsync(RemoveSubOrderCommand command, CancellationToken cancellationToken)
    {
        var currentSubOrder = await _subOrderRepo.FindOneAsync(
            x => x.OrderId == command.OrderId && x.Owner.Id == command.OwnerId,
            cancellationToken: cancellationToken);
        if (currentSubOrder is not null)
        {
            await _subOrderRepo.DeleteAsync(currentSubOrder.Id, cancellationToken);
        }

        return CommandResult.Success();
    }
}