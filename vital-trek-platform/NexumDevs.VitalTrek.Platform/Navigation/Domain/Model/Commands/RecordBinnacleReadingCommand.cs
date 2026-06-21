namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

public record RecordBinnacleReadingCommand(
    int ExpeditionId,
    int TouristId,
    string Note,
    string MediaUrl,
    DateTime CreatedAt
    );
    