namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model;

public enum NavigationError
{
    None,
    ExpeditionNotFound,
    ExperienceNotFound,
    ProgressNotLoaded,
    WeatherNotLoaded,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
