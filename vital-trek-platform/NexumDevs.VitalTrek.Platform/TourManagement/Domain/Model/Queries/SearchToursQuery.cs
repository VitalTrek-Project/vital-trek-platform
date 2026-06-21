namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;

/// <summary>
/// Query used to search for tours that match a specified search term.
/// </summary>
/// <param name="Term">
/// The search term used to find matching tours.
/// </param>
public record SearchToursQuery(string Term);