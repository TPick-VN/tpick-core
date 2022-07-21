using CsMicro.Cqrs.Commands;
using CsMicro.Persistence;
using TPick.Domain.Aggregates;

namespace TPick.App.Commands;

public class ConfirmOrderCommand : ICommand
{
    public Guid OrderId { get; init; }
}

public class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand>
{
    private readonly IGenericRepository<Order, Guid> _orderRepo;

    public ConfirmOrderCommandHandler(IGenericRepository<Order, Guid> orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async Task<CommandResult> HandleAsync(ConfirmOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.FindOneAsync(command.OrderId, cancellationToken);
        if (order is null) return CommandResult.Error("Order is not valid!");

        order.Confirm();
        await _orderRepo.SaveAsync(order, cancellationToken);

        return CommandResult.Success();
    }
}