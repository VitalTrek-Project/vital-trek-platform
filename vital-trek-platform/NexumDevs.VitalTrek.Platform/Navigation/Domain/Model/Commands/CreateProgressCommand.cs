using System.Runtime.InteropServices.JavaScript;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

public record CreateProgressCommand(
    int ExpeditionId,
    int CompletedCheckpoints,
    int TotalCheckpoints,
    double Percentage);
    