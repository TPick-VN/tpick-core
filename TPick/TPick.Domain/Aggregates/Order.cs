using CsMicro.Core;
using Newtonsoft.Json.Linq;
using TPick.Domain.ValueObjects;

namespace TPick.Domain.Aggregates;

public class Order : AggregateRoot<Guid>
{
    public override Guid Id { get; init; }
    public Guid ShopId { get; }
    public User Host { get; } = null!;
    public bool IsConfirm { get; private set; }
    public JObject Fee { get; private set; } = new();
    public JObject Discount { get; private set; } = new();

    private Order()
    {
    }

    public Order(Guid shopId, User host)
    {
        ShopId = shopId;
        Host = host;
    }

    public void Confirm()
    {
        IsConfirm = true;
    }
    
    public void Revert()
    {
        IsConfirm = false;
    }
    
    public void SetDetails(JObject fee, JObject discount)
    {
        Fee = fee;
        Discount = discount;
    }
}