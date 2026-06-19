using System.Runtime.InteropServices.JavaScript;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

public record CreateExpeditionCommand(
    TourId TourID,
    GuideId GuideID,
    string ExpeditionName,
    string Status);
    