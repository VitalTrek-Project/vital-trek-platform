namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve all tours belonging to a specific agency.
/// </summary>
/// <param name="AgencyId">
/// The unique identifier of the agency whose tours are being requested.
/// </param>
public record GetAllToursByAgencyQuery(Guid AgencyId);