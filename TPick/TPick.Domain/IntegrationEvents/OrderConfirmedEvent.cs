using CsMicro.IntegrationEvent;
using CsMicro.IntegrationEvent.Attributes;

namespace TPick.Domain.IntegrationEvents;

[PublishToExternalIntegrationEvent]
public class OrderConfirmedEvent : IIntegrationEvent
{
    public Guid OrderId { get; set; }
}