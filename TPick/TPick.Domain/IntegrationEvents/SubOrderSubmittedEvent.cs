﻿using CsMicro.IntegrationEvent;
using CsMicro.IntegrationEvent.Attributes;
using TPick.Domain.ValueObjects;

namespace TPick.Domain.IntegrationEvents;

[PublishToExternalIntegrationEvent]
public class SubOrderSubmittedEvent : IIntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid SubOrderId { get; set; }
    public User Owner { get; set; } = null!;
}