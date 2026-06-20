using System.Runtime.InteropServices.JavaScript;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

public record CreateExperienceCommand(
    int ExpeditionID,
    int TouristID,
    NoteItem Note,
    string MediaUrl);
    