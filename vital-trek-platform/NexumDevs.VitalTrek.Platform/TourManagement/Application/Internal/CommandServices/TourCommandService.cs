using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.CommandServices;

/// <summary>
/// Command service responsible for handling tour management use cases,
/// including creating, updating, deleting, duplicating tours,
/// and assigning or unassigning tourists.
/// </summary>
public class TourCommandService : ITourCommandService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourAssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="TourCommandService"/> class.
    /// </summary>
    /// <param name="tourRepository">Repository used to manage tours.</param>
    /// <param name="assignmentRepository">Repository used to manage tour assignments.</param>
    /// <param name="unitOfWork">Unit of Work used to persist changes.</param>
    /// <param name="localizer">Localizer used for localized error messages.</param>
    public TourCommandService(
        ITourRepository tourRepository,
        ITourAssignmentRepository assignmentRepository,
        IUnitOfWork unitOfWork,
        IStringLocalizer localizer)
    {
        _tourRepository = tourRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    /// <summary>
    /// Creates a new tour.
    /// </summary>
    /// <param name="command">The command containing the information required to create the tour.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The newly created <see cref="Tour"/>.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when a tour with the same title already exists for the specified agency.
    /// </exception>
    public async Task<Tour> Handle(CreateTourCommand command, CancellationToken cancellationToken)
    {
        var alreadyExists = await _tourRepository.ExistsByTitleAsync(command.Title, command.AgencyId, cancellationToken);
        if (alreadyExists)
            throw new TourManagementError(TourManagementErrors.TourTitleAlreadyExists, _localizer[TourManagementErrors.TourTitleAlreadyExists]);

        var tour = new Tour(command.AgencyId, command.Title, command.Description, command.Difficulty, command.Capacity);
        tour.SetEstimatedDuration(command.EstimatedDurationMinutes);
        tour.SetDistance(command.DistanceKm);

        await _tourRepository.AddAsync(tour, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return tour;
    }

    /// <summary>
    /// Updates an existing tour.
    /// </summary>
    /// <param name="command">The command containing the updated tour information.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The updated <see cref="Tour"/>.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the specified tour cannot be found.
    /// </exception>
    public async Task<Tour> Handle(UpdateTourCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.FindByIdAsync(command.TourId, cancellationToken);
        if (tour is null)
            throw new TourManagementError(TourManagementErrors.TourNotFound, _localizer[TourManagementErrors.TourNotFound]);

        tour.Update(command.Title, command.Description);

        _tourRepository.Update(tour);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return tour;
    }

    /// <summary>
    /// Deletes an existing tour.
    /// </summary>
    /// <param name="command">The command containing the identifier of the tour to delete.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the specified tour cannot be found.
    /// </exception>
    public async Task Handle(DeleteTourCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.FindByIdAsync(command.TourId, cancellationToken);
        if (tour is null)
            throw new TourManagementError(TourManagementErrors.TourNotFound, _localizer[TourManagementErrors.TourNotFound]);

        _tourRepository.Remove(tour);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a duplicate of an existing tour.
    /// </summary>
    /// <param name="command">The command containing the identifier of the tour to duplicate.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The duplicated <see cref="Tour"/>.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the specified tour cannot be found.
    /// </exception>
    public async Task<Tour> Handle(DuplicateTourCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.FindByIdAsync(command.TourId, cancellationToken);
        if (tour is null)
            throw new TourManagementError(TourManagementErrors.TourNotFound, _localizer[TourManagementErrors.TourNotFound]);

        var duplicated = tour.Duplicate();

        await _tourRepository.AddAsync(duplicated, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return duplicated;
    }

    /// <summary>
    /// Assigns a tourist to a tour.
    /// </summary>
    /// <param name="command">The command containing the tour and tourist identifiers.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The created <see cref="TourAssignment"/>.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the specified tour cannot be found or when the tourist is already assigned to the tour.
    /// </exception>
    public async Task<TourAssignment> Handle(AssignTouristCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.FindByIdAsync(command.TourId, cancellationToken);
        if (tour is null)
            throw new TourManagementError(TourManagementErrors.TourNotFound, _localizer[TourManagementErrors.TourNotFound]);

        var exists = await _assignmentRepository.ExistsAsync(command.TourId, command.TouristId, cancellationToken);
        if (exists)
            throw new TourManagementError(TourManagementErrors.TouristAlreadyAssigned, _localizer[TourManagementErrors.TouristAlreadyAssigned]);

        var assignment = tour.AssignTourist(command.TouristId);

        await _assignmentRepository.AddAsync(assignment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        tour.ClearDomainEvents();

        return assignment;
    }

    /// <summary>
    /// Removes a tourist assignment from a tour.
    /// </summary>
    /// <param name="command">The command containing the tour and tourist identifiers.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the specified tour cannot be found.
    /// </exception>
    public async Task Handle(UnassignTouristCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.FindByIdAsync(command.TourId, cancellationToken);
        if (tour is null)
            throw new TourManagementError(TourManagementErrors.TourNotFound, _localizer[TourManagementErrors.TourNotFound]);

        tour.UnassignTourist(command.TouristId);

        _tourRepository.Update(tour);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}