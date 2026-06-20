using System;
using Cortex.Mediator.Commands; // Or MediatR

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;

public record DismissAlertCommand(int AlertId) : ICommand; // Or IRequest