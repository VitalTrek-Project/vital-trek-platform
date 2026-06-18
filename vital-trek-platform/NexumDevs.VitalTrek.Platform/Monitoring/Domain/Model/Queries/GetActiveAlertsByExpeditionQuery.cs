using System;
using System.Collections.Generic;
using Cortex.Mediator.Queries; // Or MediatR

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;

// Assuming Alert is returned, but we might just return the ID to be resolved later, or DTO
public record GetActiveAlertsByExpeditionQuery(int ExpeditionId) : IQuery<IEnumerable<Aggregate.Alert>>; // Or IRequest<IEnumerable<Alert>>