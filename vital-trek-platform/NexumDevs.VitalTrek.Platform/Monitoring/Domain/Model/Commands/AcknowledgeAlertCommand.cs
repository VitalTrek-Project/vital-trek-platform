using System;
using Cortex.Mediator.Commands; // Or MediatR

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;

public record AcknowledgeAlertCommand(int AlertId, int UserId) : ICommand; // Or IRequest