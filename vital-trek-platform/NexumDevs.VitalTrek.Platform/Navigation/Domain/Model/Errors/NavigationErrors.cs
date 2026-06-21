using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Errors;

public static class NavigationErrors
{
    public static readonly Error ExpeditionCreationFailed =
        new("Navigation.ExpeditionCreationFailed", "An error occurred while creating the expedition.");
    
    public static readonly Error ExperienceCreationFailed =
        new("Navigation.ExperienceCreationFailed", "An error occurred while creating the experience.");
    
    public static readonly Error ProgressLoadingFailed =
        new("Navigation.ProgressLoadingFailed", "An error occurred while loading the progress.");
    
    public static readonly Error WeatherLoadingFailed =
        new("Navigation.WeatherLoadingFailed", "An error occurred while loading the weather.");
}
