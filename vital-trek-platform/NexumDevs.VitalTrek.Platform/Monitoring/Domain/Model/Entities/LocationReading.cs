using System;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;

public class LocationReading : AuditableModel
{
    public LocationReading()
    {
    }

    public LocationReading(int expeditionId, int touristId, double latitude, double longitude, double accuracyMeters)
    {
        ExpeditionId = expeditionId;
        TouristId = touristId;
        Latitude = latitude;
        Longitude = longitude;
        AccuracyMeters = accuracyMeters;
        RecordedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int ExpeditionId { get; set; }
    public int TouristId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double AccuracyMeters { get; set; }
    public DateTime RecordedAt { get; set; }

    // Calculate distance (meters) between this reading and another point using the Haversine formula
    public double DistanceTo(double otherLat, double otherLng)
    {
        double R = 6371000; // Earth radius in meters
        double latRad1 = DegreesToRadians(Latitude);
        double latRad2 = DegreesToRadians(otherLat);
        double deltaLat = DegreesToRadians(otherLat - Latitude);
        double deltaLng = DegreesToRadians(otherLng - Longitude);
double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                   Math.Cos(latRad1) * Math.Cos(latRad2) *
                   Math.Sin(deltaLng / 2) * Math.Sin(deltaLng / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double DegreesToRadians(double deg) => deg * (Math.PI / 180.0);
}