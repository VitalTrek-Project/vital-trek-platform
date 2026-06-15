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
/// Implementación de los casos de uso (comandos) del BC TourManagement.
/// Equivalente a "CategoryCommandService" / "TutorialCommandService".
/// </summary>
public class TourCommandService : ITourCommandService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourAssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

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

    public async Task Handle(DeleteTourCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.FindByIdAsync(command.TourId, cancellationToken);
        if (tour is null)
            throw new TourManagementError(TourManagementErrors.TourNotFound, _localizer[TourManagementErrors.TourNotFound]);

        _tourRepository.Remove(tour);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

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