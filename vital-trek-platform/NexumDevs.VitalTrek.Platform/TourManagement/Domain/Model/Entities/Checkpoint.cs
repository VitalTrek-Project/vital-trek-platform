namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;

/// <summary>
/// Represents a checkpoint within a tour route.
/// A checkpoint is an entity that belongs to the <c>Tour</c> aggregate
/// and defines a specific location that tourists can visit during the tour.
/// </summary>
public class Checkpoint
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Checkpoint() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Checkpoint"/> class.
    /// </summary>
    /// <param name="tourId">The identifier of the tour that owns the checkpoint.</param>
    /// <param name="order">The position of the checkpoint within the tour route.</param>
    /// <param name="name">The name of the checkpoint.</param>
    /// <param name="latitude">The geographic latitude of the checkpoint.</param>
    /// <param name="longitude">The geographic longitude of the checkpoint.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the checkpoint name is null, empty, or consists only of whitespace.
    /// </exception>
    public Checkpoint(Guid tourId, int order, string name, double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The checkpoint name is required.", nameof(name));

        Id = Guid.NewGuid();
        TourId = tourId;
        Order = order;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets the unique identifier of the checkpoint.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the tour that owns this checkpoint.
    /// </summary>
    public Guid TourId { get; private set; }

    /// <summary>
    /// Gets the position of the checkpoint within the tour route.
    /// </summary>
    public int Order { get; private set; }

    /// <summary>
    /// Gets the name of the checkpoint.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the geographic latitude of the checkpoint.
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// Gets the geographic longitude of the checkpoint.
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// Gets the Bluetooth beacon identifier associated with the checkpoint.
    /// </summary>
    public string? BluetoothBeaconId { get; private set; }

    /// <summary>
    /// Gets the descriptive text associated with the checkpoint.
    /// </summary>
    public string? DescriptionText { get; private set; }

    /// <summary>
    /// Updates the checkpoint information.
    /// </summary>
    /// <param name="name">The new checkpoint name.</param>
    /// <param name="latitude">The new geographic latitude.</param>
    /// <param name="longitude">The new geographic longitude.</param>
    /// <returns>The updated <see cref="Checkpoint"/> instance.</returns>
    public Checkpoint Update(string name, double latitude, double longitude)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        Latitude = latitude;
        Longitude = longitude;
        return this;
    }

    /// <summary>
    /// Assigns a Bluetooth beacon identifier to the checkpoint.
    /// </summary>
    /// <param name="beaconId">The Bluetooth beacon identifier.</param>
    public void SetBeacon(string beaconId) => BluetoothBeaconId = beaconId;

    /// <summary>
    /// Sets the descriptive text associated with the checkpoint.
    /// </summary>
    /// <param name="description">The description of the checkpoint.</param>
    public void SetDescription(string description) => DescriptionText = description;
}