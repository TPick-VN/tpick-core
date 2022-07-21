using CsMicro.Cqrs.Queries;
using CsMicro.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TPick.Domain.Aggregates;
using TPick.Domain.ValueObjects;

namespace TPick.App.Queries;

public class GetOrderDetailsQuery : IQuery<OrderDetails?>
{
    public Guid Id { get; init; }
}

public class GetOrderDetailsQueryHandler : IQueryHandler<GetOrderDetailsQuery, OrderDetails?>
{
    private readonly IGenericRepository<Order, Guid> _orderRepo;
    private readonly IGenericRepository<SubOrder, Guid> _subOrderRepo;

    public GetOrderDetailsQueryHandler(IGenericRepository<Order, Guid> orderRepo,
        IGenericRepository<SubOrder, Guid> subOrderRepo)
    {
        _orderRepo = orderRepo;
        _subOrderRepo = subOrderRepo;
    }

    public async Task<OrderDetails?> HandleAsync(GetOrderDetailsQuery detailsQuery, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.FindOneAsync(detailsQuery.Id, cancellationToken);
        if (order is null)
        {
            return null;
        }

        var subOrders = await _subOrderRepo.FindAsync(x => x.OrderId == order.Id, cancellationToken);
        
        return new OrderDetails()
        {
            Id = order.Id,
            Host = order.Host,
            ShopId = order.ShopId,
            Fee = order.Fee,
            Discount = order.Discount,
            SubOrders = subOrders,
        };
    }
}

public class OrderDetails
{
    public Guid Id { get; set; }
    public Guid ShopId { get; set; }
    public User Host { get; set; } = null!;
    public JObject Fee { get; set; } = new();
    public JObject Discount { get; set; } = new();
    public List<SubOrder> SubOrders { get; set; } = null!;
}