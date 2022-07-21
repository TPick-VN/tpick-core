using CsMicro.Cqrs.Commands;
using CsMicro.Persistence;
using Microsoft.EntityFrameworkCore;
using TPick.Domain.Aggregates;
using TPick.Domain.ValueObjects;

namespace TPick.App.Commands;

public class SubmitSubOrderCommand : ICommand
{
    public Guid OrderId { get; set; }
    public User Owner { get; init; }
    public string? Note { get; init; }
    public List<SubOrder.OrderItem> Items { get; init; }
}

public class SubmitSubOrderCommandHandler : ICommandHandler<SubmitSubOrderCommand>
{
    private readonly IGenericRepository<Order, Guid> _orderRepo;
    private readonly IGenericRepository<SubOrder, Guid> _subOrderRepo;

    public SubmitSubOrderCommandHandler(IGenericRepository<Order, Guid> orderRepo,
        IGenericRepository<SubOrder, Guid> subOrderRepo)
    {
        _orderRepo = orderRepo;
        _subOrderRepo = subOrderRepo;
    }

    public async Task<CommandResult> HandleAsync(SubmitSubOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.FindOneAsync(command.OrderId, cancellationToken);
        if (order is null)
        {
            return CommandResult.Error("Order is not valid!");
        }

        var currentSubOrder = await _subOrderRepo.FindOneAsync(
            x => x.OrderId == command.OrderId && x.Owner.Id == command.Owner.Id,
            cancellationToken);
        if (currentSubOrder is not null)
        {
            await _subOrderRepo.DeleteAsync(currentSubOrder.Id, cancellationToken);
        }

        var subOrder = new SubOrder(command.OrderId, command.Owner, command.Note, command.Items);
        await _subOrderRepo.AddAsync(subOrder, cancellationToken);

        return CommandResult.Success();
    }
}