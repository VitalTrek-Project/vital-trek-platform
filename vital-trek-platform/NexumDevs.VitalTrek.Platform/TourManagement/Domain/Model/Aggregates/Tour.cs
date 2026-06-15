using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root of the TourManagement Bounded Context.
/// Represents a tour offered by an agency, including its
/// checkpoints (route) and tourist assignments.
/// </summary>
public class Tour
{
    private readonly List<Checkpoint> _checkpoints = new();
    private readonly List<TourAssignment> _assignments = new();
    private readonly List<object> _domainEvents = new();

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Tour() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tour"/> class.
    /// </summary>
    /// <param name="agencyId">The identifier of the agency that owns the tour.</param>
    /// <param name="title">The title of the tour.</param>
    /// <param name="description">The description of the tour.</param>
    /// <param name="difficulty">The difficulty level of the tour.</param>
    /// <param name="capacity">The maximum number of tourists allowed on the tour.</param>
    /// <exception cref="TourManagementError">
    /// Thrown when the title is empty or the capacity is less than or equal to zero.
    /// </exception>
    public Tour(Guid agencyId, string title, string description, EDifficultyLevel difficulty, int capacity)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new TourManagementError(TourManagementErrors.InvalidTourCapacity, "El título del tour es obligatorio.");

        if (capacity <= 0)
            throw new TourManagementError(TourManagementErrors.InvalidTourCapacity, "La capacidad debe ser mayor a cero.");

        Id = Guid.NewGuid();
        AgencyId = agencyId;
        Title = title;
        Description = description;
        Difficulty = difficulty;
        Capacity = capacity;
        Status = ETourStatus.Draft;
        CreatedDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of the tour.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the agency that owns the tour.
    /// </summary>
    public Guid AgencyId { get; private set; }

    /// <summary>
    /// Gets the title of the tour.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the description of the tour.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the difficulty level of the tour.
    /// </summary>
    public EDifficultyLevel Difficulty { get; private set; }

    /// <summary>
    /// Gets the current status of the tour.
    /// </summary>
    public ETourStatus Status { get; private set; }

    /// <summary>
    /// Gets the maximum number of tourists allowed on the tour.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Gets the estimated duration of the tour in minutes.
    /// </summary>
    public int EstimatedDurationMinutes { get; private set; }

    /// <summary>
    /// Gets the total distance of the tour in kilometers.
    /// </summary>
    public decimal DistanceKm { get; private set; }

    /// <summary>
    /// Gets the date and time when the tour was created.
    /// </summary>
    public DateTimeOffset? CreatedDate { get; private set; }

    /// <summary>
    /// Gets the date and time when the tour was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; private set; }

    /// <summary>
    /// Gets the collection of checkpoints that define the tour route.
    /// </summary>
    public IReadOnlyCollection<Checkpoint> Checkpoints => _checkpoints.AsReadOnly();

    /// <summary>
    /// Gets the collection of tourist assignments associated with the tour.
    /// </summary>
    public IReadOnlyCollection<TourAssignment> Assignments => _assignments.AsReadOnly();

    /// <summary>
    /// Gets the collection of domain events pending publication.
    /// The application layer reads these events after persistence
    /// and clears them using <see cref="ClearDomainEvents"/>.
    /// </summary>
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all pending domain events.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Updates the basic information of the tour.
    /// This operation is only allowed when the tour is not cancelled.
    /// </summary>
    /// <param name="title">The new title of the tour.</param>
    /// <param name="description">The new description of the tour.</param>
    /// <returns>The updated <see cref="Tour"/> instance.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when attempting to update a cancelled tour.
    /// </exception>
    public Tour Update(string title, string description)
    {
        if (Status == ETourStatus.Cancelled)
            throw new TourManagementError(TourManagementErrors.TourCannotBeUpdated, "No se puede actualizar un tour cancelado.");

        if (!string.IsNullOrWhiteSpace(title))
            Title = title;

        if (description is not null)
            Description = description;

        UpdatedDate = DateTimeOffset.UtcNow;
        return this;
    }

    /// <summary>
    /// Publishes the tour, making it visible and available for tourist assignments.
    /// </summary>
    /// <returns>The current <see cref="Tour"/> instance.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the tour is not in draft status.
    /// </exception>
    public Tour Publish()
    {
        if (Status != ETourStatus.Draft)
            throw new TourManagementError(TourManagementErrors.TourCannotBePublished, "Solo un tour en borrador puede publicarse.");

        Status = ETourStatus.Available;
        UpdatedDate = DateTimeOffset.UtcNow;

        _domainEvents.Add(new TourPublishedEvent(Id, AgencyId, UpdatedDate.Value));
        return this;
    }

    /// <summary>
    /// Closes the tour and prevents new tourist assignments.
    /// </summary>
    /// <returns>The current <see cref="Tour"/> instance.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the tour is not currently available.
    /// </exception>
    public Tour Close()
    {
        if (Status != ETourStatus.Available)
            throw new TourManagementError(TourManagementErrors.TourCannotBeClosed, "Solo un tour disponible puede cerrarse.");

        Status = ETourStatus.Closed;
        UpdatedDate = DateTimeOffset.UtcNow;
        return this;
    }

    /// <summary>
    /// Cancels the tour.
    /// </summary>
    /// <returns>The current <see cref="Tour"/> instance.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the tour has already been cancelled.
    /// </exception>
    public Tour Cancel()
    {
        if (Status == ETourStatus.Cancelled)
            throw new TourManagementError(TourManagementErrors.TourAlreadyCancelled, "El tour ya está cancelado.");

        Status = ETourStatus.Cancelled;
        UpdatedDate = DateTimeOffset.UtcNow;
        return this;
    }

    /// <summary>
    /// Creates a draft copy of the current tour without checkpoints
    /// or tourist assignments.
    /// </summary>
    /// <returns>A duplicated <see cref="Tour"/> instance.</returns>
    public Tour Duplicate()
    {
        var copy = new Tour(AgencyId, $"{Title} (copia)", Description, Difficulty, Capacity)
        {
            EstimatedDurationMinutes = EstimatedDurationMinutes,
            DistanceKm = DistanceKm
        };
        return copy;
    }

    // ---- Operaciones sobre Checkpoints ----

    /// <summary>
    /// Adds a checkpoint to the tour route.
    /// </summary>
    /// <param name="order">The position of the checkpoint in the route.</param>
    /// <param name="name">The checkpoint name.</param>
    /// <param name="latitude">The geographic latitude of the checkpoint.</param>
    /// <param name="longitude">The geographic longitude of the checkpoint.</param>
    /// <returns>The created <see cref="Checkpoint"/>.</returns>
    public Checkpoint AddCheckpoint(int order, string name, double latitude, double longitude)
    {
        var checkpoint = new Checkpoint(Id, order, name, latitude, longitude);
        _checkpoints.Add(checkpoint);
        return checkpoint;
    }

    // ---- Operaciones sobre Assignments ----

    /// <summary>
    /// Assigns a tourist to the tour, validating availability,
    /// capacity, and duplicate assignments.
    /// </summary>
    /// <param name="touristId">The identifier of the tourist.</param>
    /// <returns>The created <see cref="TourAssignment"/>.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the tour is unavailable, the tourist is already assigned,
    /// or the tour has reached its maximum capacity.
    /// </exception>
    public TourAssignment AssignTourist(Guid touristId)
    {
        if (Status != ETourStatus.Available)
            throw new TourManagementError(TourManagementErrors.TourNotAvailableForAssignment, "Solo se pueden asignar turistas a un tour disponible.");

        if (_assignments.Any(a => a.TouristId == touristId && a.Status != EAssignmentStatus.Cancelled))
            throw new TourManagementError(TourManagementErrors.TouristAlreadyAssigned, "El turista ya está asignado a este tour.");

        var activeAssignments = _assignments.Count(a => a.Status != EAssignmentStatus.Cancelled);
        if (activeAssignments >= Capacity)
            throw new TourManagementError(TourManagementErrors.TourCapacityExceeded, "El tour ha alcanzado su capacidad máxima.");

        var assignment = new TourAssignment(Id, touristId);
        _assignments.Add(assignment);

        _domainEvents.Add(new TouristAssignedEvent(Id, touristId, assignment.AssignedAt));
        return assignment;
    }

    /// <summary>
    /// Removes a tourist assignment from the tour.
    /// </summary>
    /// <param name="touristId">The identifier of the tourist to unassign.</param>
    /// <exception cref="TourManagementError">
    /// Thrown when the tourist is not assigned to the tour.
    /// </exception>
    public void UnassignTourist(Guid touristId)
    {
        var assignment = _assignments.FirstOrDefault(a => a.TouristId == touristId);
        if (assignment is null)
            throw new TourManagementError(TourManagementErrors.AssignmentNotFound, "El turista no está asignado a este tour.");

        assignment.Cancel();
    }

    /// <summary>
    /// Sets the estimated duration of the tour.
    /// Intended for internal use by the application layer.
    /// </summary>
    /// <param name="minutes">The estimated duration in minutes.</param>
    internal void SetEstimatedDuration(int minutes) => EstimatedDurationMinutes = minutes;

    /// <summary>
    /// Sets the total distance of the tour.
    /// Intended for internal use by the application layer.
    /// </summary>
    /// <param name="distanceKm">The distance in kilometers.</param>
    internal void SetDistance(decimal distanceKm) => DistanceKm = distanceKm;
}