using CsMicro.IntegrationEvent;
using CsMicro.IntegrationEvent.Attributes;

namespace TPick.Domain.IntegrationEvents;

[PublishToExternalIntegrationEvent]
public class OrderConfirmationRevertedEvent : IIntegrationEvent
{
    public Guid OrderId { get; set; }
}