using CsMicro.Core;
using TPick.Domain.ValueObjects;

namespace TPick.Domain.Aggregates;

public class SubOrder : AggregateRoot<Guid>
{
    public override Guid Id { get; init; } = Guid.NewGuid();
    public Guid OrderId { get; }
    public User Owner { get; } = null!;
    public string? Note { get; }
    public List<OrderItem> Items { get; }

    private SubOrder()
    {
    }

    public SubOrder(Guid orderId, User owner, string? note, List<OrderItem> items)
    {
        OrderId = orderId;
        Owner = owner;
        Note = note;
        Items = items;
    }

    public class OrderItem
    {
        public string Name { get; } = null!;
        public int Quantity { get; }
        public decimal Price { get; }

        private OrderItem()
        {
        }

        public OrderItem(string name, int quantity, decimal price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}