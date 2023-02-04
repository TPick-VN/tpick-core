using CsMicro.Cqrs.Commands;
using CsMicro.IntegrationEvent;
using CsMicro.Persistence;
using TPick.Domain.Aggregates;
using TPick.Domain.IntegrationEvents;

namespace TPick.App.Commands;

public class RemoveSubOrderCommand : ICommand
{
    public Guid OrderId { get; set; }
    public Guid OwnerId { get; init; }
}

public class RemoveSubOrderCommandHandler : ICommandHandler<RemoveSubOrderCommand>
{
    private readonly IGenericRepository<SubOrder, Guid> _subOrderRepo;
    private readonly IEventPublisher _eventPublisher;

    public RemoveSubOrderCommandHandler(IGenericRepository<SubOrder, Guid> subOrderRepo, IEventPublisher eventPublisher)
    {
        _subOrderRepo = subOrderRepo;
        _eventPublisher = eventPublisher;
    }

    public async Task<CommandResult> HandleAsync(RemoveSubOrderCommand command, CancellationToken cancellationToken)
    {
        var currentSubOrder = await _subOrderRepo.FindOneAsync(
            x => x.OrderId == command.OrderId && x.Owner.Id == command.OwnerId,
            cancellationToken: cancellationToken);
        if (currentSubOrder is null) return CommandResult.Success();

        await _subOrderRepo.DeleteAsync(currentSubOrder.Id, cancellationToken);
        
        var @event = new SubOrderRemovedEvent()
        {
            OrderId = currentSubOrder.OrderId,
            SubOrderId = currentSubOrder.Id,
            Owner = currentSubOrder.Owner,
        };
        await _eventPublisher.PublishAsync(@event, cancellationToken);

        return CommandResult.Success();
    }
}