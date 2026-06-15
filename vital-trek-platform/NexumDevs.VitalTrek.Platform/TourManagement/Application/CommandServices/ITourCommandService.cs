using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.CommandServices;

/// <summary>
/// Defines the contract for handling tour-related commands,
/// including creating, updating, deleting, duplicating tours,
/// and managing tourist assignments.
/// </summary>
public interface ITourCommandService
{
    /// <summary>
    /// Creates a new tour.
    /// </summary>
    /// <param name="command">The command containing the information required to create the tour.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The newly created <see cref="Tour"/>.</returns>
    Task<Tour> Handle(CreateTourCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing tour.
    /// </summary>
    /// <param name="command">The command containing the updated tour information.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The updated <see cref="Tour"/>.</returns>
    Task<Tour> Handle(UpdateTourCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an existing tour.
    /// </summary>
    /// <param name="command">The command containing the identifier of the tour to delete.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(DeleteTourCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a duplicate of an existing tour.
    /// </summary>
    /// <param name="command">The command containing the identifier of the tour to duplicate.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The duplicated <see cref="Tour"/>.</returns>
    Task<Tour> Handle(DuplicateTourCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Assigns a tourist to a tour.
    /// </summary>
    /// <param name="command">The command containing the tour and tourist identifiers.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The created <see cref="TourAssignment"/>.</returns>
    Task<TourAssignment> Handle(AssignTouristCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a tourist assignment from a tour.
    /// </summary>
    /// <param name="command">The command containing the tour and tourist identifiers.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(UnassignTouristCommand command, CancellationToken cancellationToken);
}