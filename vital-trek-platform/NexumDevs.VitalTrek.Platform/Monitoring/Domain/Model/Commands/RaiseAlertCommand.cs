using System;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.ValueObjects;
using Cortex.Mediator.Commands; // Or MediatR if you are using it instead

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;

public record RaiseAlertCommand(
    int ExpeditionId, // Changed to int to match the pattern from Incident if Guid is not used globally
    int TouristId,
    string Type,
    string Severity,
    string Message
) : ICommand; // Or IRequest if using MediatR