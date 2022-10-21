using CsMicro.IntegrationEvent;
using CsMicro.IntegrationEvent.Attributes;
using TPick.Domain.ValueObjects;

namespace TPick.Domain.IntegrationEvents;

[PublishToExternalIntegrationEvent]
public class SubOrderRemovedEvent : IIntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid SubOrderId { get; set; }
    public SubOrderOwner Owner { get; set; } = null!;
}