using System.Runtime.InteropServices.JavaScript;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

public record CreateExpeditionCommand(
    int TourID,
    int GuideID,
    string ExpeditionName,
    string Status);
    