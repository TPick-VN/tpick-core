using System.Text.Json.Serialization;
using CsMicro.Cqrs.Commands;
using CsMicro.Persistence;
using TPick.Domain.Aggregates;
using TPick.Domain.ValueObjects;

namespace TPick.App.Commands;

public class CreateOrderCommand : ICommand
{
    public Guid ShopId { get; init; }
    public User Host { get; init; }
    [JsonIgnore] public Action<Guid>? OnOrderId { get; set; }
}

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly IGenericRepository<Order, Guid> _orderRepo;

    public CreateOrderCommandHandler(IGenericRepository<Order, Guid> orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async Task<CommandResult> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order(command.ShopId, command.Host);
        await _orderRepo.AddAsync(order, cancellationToken);
        command.OnOrderId?.Invoke(order.Id);

        return CommandResult.Success();
    }
}